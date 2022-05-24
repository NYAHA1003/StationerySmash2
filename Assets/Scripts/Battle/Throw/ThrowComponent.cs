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

        //인스펙터 참조 변수
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
        private PencilCaseDataSO _playerPencilCaseDataSO = null;

        //참조 변수
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
        /// 초기화
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
            this._throwSelectComponent.SetInitialization(this, _unitCommand);
            this._throwGaugeComponent.SetInitialization(this, _unitCommand, _throwBarFrame, _throwGaugeBar, _playerPencilCaseDataSO);

            updateAction += UpdateThrowGauge;
        }

        /// <summary>
        /// 유닛 던지기
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

                _throwParabolaComponent.UnSetDirectionArrow();
            }
            _dirArrow.gameObject.SetActive(false);
        }

        /// <summary>
        /// 던질 유닛 터치 & 선택
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
        }

        /// <summary>
        /// 던질 유닛을 당기는 중일 때
        /// </summary>
        /// <param name="pos"></param>
        public void PullingThrowUnit(Vector2 pos)
        {
            UnDrawParabola();
            if (_throwedUnit != null)
            {
                //방향
                _direction = (Vector2)_throwedUnit.transform.position - pos;
                float dir = Mathf.Atan2(_direction.y, _direction.x);
                float dirx = Mathf.Atan2(_direction.y, -_direction.x);

                //초기 벡터
                _force = Mathf.Clamp(Vector2.Distance(_throwedUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwedUnit.UnitStat.Return_Weight());

                //화살표 설정
                _throwParabolaComponent.SetDirectionArrow(_force);

                //시간이 지나면 취소
                _pullTime -= Time.deltaTime;
                if (_pullTime < 0)
                {
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

                //유닛이 다른 행동을 취하게 되면 취소
                if (_throwedUnit.Pulling_Unit() == null)
                {
                    _throwedUnit.UnitSprite.OrderDraw(_throwedUnit.OrderIndex);
                    _throwedUnit.UnitSticker.OrderDraw(_throwedUnit.OrderIndex);
                    _parabolaBackground.SetActive(false);
                    UnDrawParabola();
                    _throwedUnit = _throwedUnit.Pulling_Unit();
                    _cameraCommand.SetIsDontMove(false);
                    _throwParabolaComponent.UnSetDirectionArrow();
                    return;
                }


                //던지는 방향에 따라 포물선만 안 보이게 함
                if (dir < 0)
                {
                    UnDrawParabola();
                    return;
                }

                //수평 도달 거리
                float width = Parabola.Caculated_Width(_force, dirx);
                //수평 도달 시간
                float time = Parabola.Caculated_Time(_force, dir, 2);

                SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

                return;
            }
        }

        /// <summary>
        /// 던져진 유닛의 던지기가 끝났을 때 던지기 관련 기능들을 끝낸다
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
        /// 던질 유닛 설정
        /// </summary>
        /// <param name="unit"></param>
        public void SetThrowedUnit(Unit unit)
		{
            _throwedUnit = unit;
        }

        /// <summary>
        /// 던질 유닛 가져오기
        /// </summary>
        /// <param name="unit"></param>
        public void GetThrowedUnit(Unit unit)
        {
            _throwedUnit = unit;
        }
        
        /// <summary>
        /// 방향 반환
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDirection()
		{
            return _direction;
		}

        /// <summary>
        /// 포물선 그리기 해제
        /// </summary>
        private void UnDrawParabola()
        {
            _throwParabolaComponent.UnDrawParabola();
        }

        /// <summary>
        /// 라인렌더러의 포물선 위치 설정
        /// </summary>
        /// <param name="linePos"></param>
        private void SetParabolaPos(int count, float width, float force, float radDir, float time)
        {
            _throwParabolaComponent.SetParabolaPos(count, width, force, radDir, time);
        }

        /// <summary>
        /// 던지기 게이지 증감
        /// </summary>
        /// <param name="add"></param>
        private void IncreaseThrowGauge(float add)
        {
            _throwGaugeComponent.IncreaseThrowGauge(add);
        }

        /// <summary>
        /// 업데이트 게이지
        /// </summary>
        private void UpdateThrowGauge()
        {
            _throwGaugeComponent.UpdateThrowGauge();
        }
    }

}
