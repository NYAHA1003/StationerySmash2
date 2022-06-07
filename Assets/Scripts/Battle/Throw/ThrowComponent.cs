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
        private Transform _parabolaArrow;
        [SerializeField]
        private Transform _dirArrow;
        [SerializeField]
        private RectTransform _throwBarFrame;
        [SerializeField]
        private RectTransform _throwGaugeBar;
        [SerializeField]
        private GameObject _parabolaBackground = null;
        [SerializeField]
        private TrailRenderer _throwTrail = null;
        [SerializeField]
        private GameObject _delayBar;
        [SerializeField]
        private GameObject _delayHalfBar;
        [SerializeField]
        private SpriteMask _delayMaskPart;
        [SerializeField]
        private SpriteMask _delayMask;
        [SerializeField]
        private SpriteRenderer _delayBarImage;

        //���� ����
        private Unit _throwedUnit = null;
        private StageData _stageData = null;
        private UnitComponent _unitCommand = null;
        private CameraComponent _cameraCommand = null;
        private ThrowParabolaComponent _throwParabolaComponent = null;
        private ThrowSelectComponent _throwSelectComponent = null;
        private ThrowGaugeComponent _throwGaugeComponent = null;

        private Vector2 _direction;
        private float _force;
        private float _pullTime;

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
            this._throwGaugeComponent = new ThrowGaugeComponent();

            this._unitCommand = unitCommand;
            this._cameraCommand = cameraCommand;
            this._stageData = stageData;

            this._throwParabolaComponent.SetInitialization(_parabola, this, _stageData, _parabolaBackground, _cameraCommand, _parabolaArrow, _dirArrow);
            this._throwSelectComponent.SetInitialization(this, _throwGaugeComponent, _unitCommand);
            this._throwGaugeComponent.SetInitialization(this, _unitCommand, _throwBarFrame, _throwGaugeBar, PencilCaseDataManagerSO.InGamePencilCaseData);
            SetDelayBar();
            UnSetDelayBar();

            updateAction += UpdateThrowGauge;
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public void ThrowUnit()
        {
            UnSetDelayBar();
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

                _throwParabolaComponent.UnSetDirectionArrow();
            }
            _dirArrow.gameObject.SetActive(false);
        }

        /// <summary>
        /// �����̹� ����
        /// </summary>
        public void SetDelayBar()
        {
            _delayMaskPart.gameObject.SetActive(true);
            _delayMaskPart.transform.rotation = Quaternion.Euler(0, 0, 180);
            _delayMask.transform.rotation = Quaternion.identity;
            _delayBarImage.transform.rotation = Quaternion.identity;
            _delayBar.SetActive(true);
            _delayHalfBar.SetActive(false);
        }

        /// <summary>
        /// �����̹� �����
        /// </summary>
        public void UnSetDelayBar()
		{
            _delayBar.SetActive(false);
            _delayHalfBar.SetActive(false);
        }

        /// <summary>
        /// �����̹� ������Ʈ
        /// </summary>
        /// <param name="delay"></param>
        public void UpdateDelayBar(float delay)
        {
            _delayBar.transform.position = _throwedUnit.transform.position;
            _delayHalfBar.transform.position = _throwedUnit.transform.position;
            _delayMask.transform.rotation = Quaternion.Euler(0, 0, delay * 360);

            if (delay >= 0.5f)
            {
                _delayMaskPart.gameObject.SetActive(false);
                _delayMask.gameObject.SetActive(true);
                _delayHalfBar.SetActive(true);
            }
            else
            {
                _delayMaskPart.gameObject.SetActive(true);
                _delayMask.gameObject.SetActive(true);
                _delayHalfBar.SetActive(false);
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
            if (_throwGaugeComponent.GetThrowGauge() < _throwedUnit.UnitStat.Return_Weight())
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
            SetDelayBar();
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
                //����
                _direction = (Vector2)_throwedUnit.transform.position - pos;
                float dir = Mathf.Atan2(_direction.y, _direction.x);
                float dirx = Mathf.Atan2(_direction.y, -_direction.x);

                //�ʱ� ����
                _force = Mathf.Clamp(Vector2.Distance(_throwedUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwedUnit.UnitStat.Return_Weight());

                //ȭ��ǥ ����
                _throwParabolaComponent.SetDirectionArrow(_force);

                //�ð��� ������ ���
                _pullTime -= Time.deltaTime;
                UpdateDelayBar(_pullTime / 2);
                if (_pullTime < 0)
                {
                    UnSetDelayBar();
                    _throwedUnit.UnitSprite.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit.UnitSticker.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit = null;
                    _parabolaBackground.SetActive(false);
                    _cameraCommand.SetIsDontMove(false);
                    UnDrawParabola();
                    _throwParabolaComponent.UnSetDirectionArrow();
                    return;
                }

                _cameraCommand.SetIsDontMove(true);

                //������ �ٸ� �ൿ�� ���ϰ� �Ǹ� ���
                if (_throwedUnit.Pulling_Unit() == null)
                {
                    UnSetDelayBar();
                    _throwedUnit.UnitSprite.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit.UnitSticker.OrderDraw(_throwedUnit.OrderIndex);
                    _parabolaBackground.SetActive(false);
                    UnDrawParabola();
                    _throwedUnit = _throwedUnit.Pulling_Unit();
                    _cameraCommand.SetIsDontMove(false);
                    _throwParabolaComponent.UnSetDirectionArrow();
                    return;
                }


                //������ ���⿡ ���� �������� �� ���̰� ��
                if (dir < 0)
                {
                    UnDrawParabola();
                    return;
                }

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
        /// ���� ���� ��������
        /// </summary>
        /// <param name="unit"></param>
        public void GetThrowedUnit(Unit unit)
        {
            _throwedUnit = unit;
        }
        
        /// <summary>
        /// ���� ��ȯ
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDirection()
		{
            return _direction;
		}

        /// <summary>
        /// ������ �׸��� ����
        /// </summary>
        private void UnDrawParabola()
        {
            _throwParabolaComponent.UnDrawParabola();
        }

        /// <summary>
        /// ���η������� ������ ��ġ ����
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
            _throwGaugeComponent.IncreaseThrowGauge(add);
        }

        /// <summary>
        /// ������Ʈ ������
        /// </summary>
        private void UpdateThrowGauge()
        {
            _throwGaugeComponent.UpdateThrowGauge();
        }
    }

}
