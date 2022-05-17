using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

namespace Battle
{
    [System.Serializable]
    public class ThrowComponent : BattleComponent
    {
        public Unit ThrowedUnit => _throwedUnit;

        //�ν����� ���� ����
        [SerializeField]
        private LineRenderer _parabola;
        [SerializeField]
        private Transform _arrow;
        [SerializeField]
        private RectTransform _throwBarFrame;
        [SerializeField]
        private RectTransform _throwDelayBar;
        [SerializeField]
        private GameObject _parabolaBackground = null;
        [SerializeField]
        private TrailRenderer _throwTrail = null;
        [SerializeField]
        private PencilCaseDataSO _playerPencilCaseDataSO = null;

        //���� ����
        private Unit _throwedUnit = null;
        private StageData _stageData = null;
        private UnitComponent _unitCommand = null;
        private CameraComponent _cameraCommand = null;
        private ThrowParabolaComponent _throwParabolaComponent = null;
        private ThrowSelectComponent _throwSelectComponent = null;

        private Vector2 _direction;
        private float _force;
        private float _pullTime;
        private float _throwGauge = 0f;
        private float _throwGaugeSpeed = 0f;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="parabola"></param>
        /// <param name="arrow"></param>
        /// <param name="stageData"></param>
        public void SetInitialization(ref System.Action updateAction, UnitComponent unitCommand, CameraComponent cameraCommand, StageData stageData)
        {
            this._throwParabolaComponent = new ThrowParabolaComponent();
            this._throwSelectComponent = new ThrowSelectComponent();

            this._unitCommand = unitCommand;
            this._cameraCommand = cameraCommand;
            this._stageData = stageData;
            this._throwGaugeSpeed = _playerPencilCaseDataSO._pencilCaseData._throwGaugeSpeed;

            this._throwParabolaComponent.SetInitialization(_parabola, this, _stageData, _parabolaBackground, _cameraCommand, _arrow);
            this._throwSelectComponent.SetInitialization(this, _unitCommand);

            updateAction += UpdateThrowDelay;
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public void ThrowUnit()
        {
            if (_throwedUnit != null)
            {
                _throwedUnit.Throw_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                IncreaseThrowGauge(-_throwedUnit.UnitStat.Return_Weight());
                _throwTrail.transform.SetParent(_throwedUnit.transform);
                _throwTrail.transform.localPosition = Vector2.zero;
                _throwTrail.Clear();
                _parabolaBackground.SetActive(false);
                _cameraCommand.SetIsDontMove(false);
                UnDrawParabola();
            }
        }

        /// <summary>
        /// ���� ���� ��ġ & ����
        /// </summary>
        /// <param name="pos"></param>
        public void ClickThrowUnit(Vector2 pos)
        {
            _throwSelectComponent.SelectThrowUnit(pos);
            
            if(_throwedUnit == null)
			{
                return;
			}
            if (_throwGauge < _throwedUnit.UnitStat.Return_Weight())
            {
                _throwedUnit = null;
                return;
            }
            if (_throwedUnit == null)
            {
                _cameraCommand.SetIsDontMove(true);
                return;
            }
            else
            {
                _throwedUnit.UnitSprite.OrderDraw(-10);
                _throwedUnit.UnitSticker.OrderDraw(-10);
                _parabolaBackground.SetActive(true);
                _cameraCommand.SetIsDontMove(false);
            }

            _pullTime = 2f;
        }

        /// <summary>
        /// ���� ������ ���� ���� ��
        /// </summary>
        /// <param name="pos"></param>
        public void PullingThrowUnit(Vector2 pos)
        {
            UnDrawParabola();
            if (_throwedUnit != null)
            {
                //�ð��� ������ ���
                _pullTime -= Time.deltaTime;
                if (_pullTime < 0)
                {
                    _throwedUnit.UnitSprite.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit.UnitSticker.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit = null;
                    _parabolaBackground.SetActive(false);
                    _cameraCommand.SetIsDontMove(false);
                    UnDrawParabola();
                    return;
                }

                _cameraCommand.SetIsDontMove(true);

                //������ �ٸ� �ൿ�� ���ϰ� �Ǹ� ���
                if (_throwedUnit.Pulling_Unit() == null)
                {
                    _throwedUnit.UnitSprite.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit.UnitSticker.OrderDraw(_throwedUnit.OrderIndex);
                    _parabolaBackground.SetActive(false);
                    UnDrawParabola();
                    _throwedUnit = _throwedUnit.Pulling_Unit();
                    _cameraCommand.SetIsDontMove(false);
                    return;
                }

                //����
                _direction = (Vector2)_throwedUnit.transform.position - pos;
                float dir = Mathf.Atan2(_direction.y, _direction.x);
                float dirx = Mathf.Atan2(_direction.y, -_direction.x);

                //������ ���⿡ ���� �������� �� ���̰� ��
                if (dir < 0)
                {
                    UnDrawParabola();
                    return;
                }

                //ȭ��ǥ
                _arrow.transform.position = _throwedUnit.transform.position;
                _arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);

                //�ʱ� ����
                _force = Mathf.Clamp(Vector2.Distance(_throwedUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwedUnit.UnitStat.Return_Weight());

                //���� ���� �Ÿ�
                float width = Parabola.Caculated_Width(_force, dirx);
                //���� ���� �ð�
                float time = Parabola.Caculated_Time(_force, dir, 2);

                SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

                return;
            }
        }

        /// <summary>
        /// ������ ������ �����Ⱑ ������ �� ������ ���� ��ɵ��� ������
        /// </summary>
        /// <param name="unit"></param>
        public void EndThrowUnit(Unit unit)
        {
            if (unit == _throwedUnit)
            {
                _throwedUnit = null;
                _throwTrail.transform.SetParent(null);
            }
        }

        /// <summary>
        /// ���� ���� ����
        /// </summary>
        /// <param name="unit"></param>
        public void SetThrowedUnit(Unit unit)
		{
            _throwedUnit = unit;
        }
        /// <summary>
        /// ���� ���� ����
        /// </summary>
        /// <param name="unit"></param>
        public void GetThrowedUnit(Unit unit)
        {
            _throwedUnit = unit;
        }

        /// <summary>
        /// ������ �׸��� ����
        /// </summary>
        private void UnDrawParabola()
        {
            _throwParabolaComponent.UnDrawParabola();
        }

        /// <summary>
        /// ���η������� ������ ��ġ �༭ ����
        /// </summary>
        /// <param name="linePos"></param>
        private void SetParabolaPos(int count, float width, float force, float radDir, float time)
        {
            _throwParabolaComponent.SetParabolaPos(count, width, force, radDir, time);
        }

        /// <summary>
        /// ������ ������ ����
        /// </summary>
        /// <param name="add"></param>
        private void IncreaseThrowGauge(float add)
        {
            _throwGauge += add;
            if(_throwGauge < 0)
            {
                _throwGauge = 0;
            }
            else if(_throwGauge > 200)
            {
                _throwGauge = 200;
            }
        }

        /// <summary>
        /// ������ ������ ���ֵ��� �ð��� ȿ���� �����Ѵ�.
        /// </summary>
        private void CheckCanThrow()
        {
            int count = _unitCommand._playerUnitList.Count;
            for (int i = 1; i < count; i++)
            {
                _unitCommand._playerUnitList[i].SetThrowRenderer(_throwGauge);
            }
        }

        /// <summary>
        /// ������Ʈ ������
        /// </summary>
        private void UpdateThrowDelay()
        {
            if (_throwGauge <= 200f)
            {
                IncreaseThrowGauge(Time.deltaTime * _throwGaugeSpeed);
                Vector2 rectSize = _throwDelayBar.sizeDelta;
                rectSize.x = _throwBarFrame.rect.width * (_throwGauge / 200f);
                _throwDelayBar.sizeDelta = rectSize;
                CheckCanThrow();
            }
        }
    }

}
