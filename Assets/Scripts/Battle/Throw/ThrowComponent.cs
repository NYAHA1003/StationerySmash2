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

        //참조 변수
        private Unit _throwedUnit = null;
        private StageData _stageData = null;
        private UnitComponent _unitCommand = null;
        private CameraComponent _cameraCommand = null;
        private ThrowParabolaComponent _throwParabolaComponent = null;

        private List<Vector2> _lineZeroPos;
        private Vector2 _direction;
        private float _force;
        private float _pullTime;
        private float _throwGauge = 0f;
        private float _throwGaugeSpeed = 0f;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="parabola"></param>
        /// <param name="arrow"></param>
        /// <param name="stageData"></param>
        public void SetInitialization(ref System.Action updateAction, UnitComponent unitCommand, CameraComponent cameraCommand, StageData stageData)
        {
            _unitCommand = unitCommand;
            _cameraCommand = cameraCommand;
            this._stageData = stageData;
            _lineZeroPos = new List<Vector2>(_parabola.positionCount);
            _throwGaugeSpeed = _playerPencilCaseDataSO._pencilCaseData._throwGaugeSpeed;
            for (int i = 0; i < _parabola.positionCount; i++)
            {
                _lineZeroPos.Add(Vector2.zero);
            }

            updateAction += UpdateThrowDelay;
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
            }
        }

        /// <summary>
        /// 던지기가 끝났을 때 선택된 유닛을 Null로 바꾼다
        /// </summary>
        /// <param name="unit"></param>
        public void EndThrowTarget(Unit unit)
        {
            if (unit == _throwedUnit)
            {
                _throwedUnit = null;
                _throwTrail.transform.SetParent(null);
            }
        }

        /// <summary>
        /// 던질 유닛 선택
        /// </summary>
        /// <param name="pos"></param>
        public void PullUnit(Vector2 pos)
        {
            int firstNum = 0;
            int lastNum = _unitCommand._playerUnitList.Count - 1;
            int loopnum = 0;
            int count = _unitCommand._playerUnitList.Count;
            List<Unit> list = _unitCommand._playerUnitList;
            float targetPosX = 0;
            _throwedUnit = null;
            if(list.Count == 0)
			{
                return;
			}

            if(pos.x >= list[lastNum].transform.position.x - 0.3f)
            {
                _throwedUnit = list[lastNum];
            }
            else if (pos.x <= list[firstNum].transform.position.x )
            {
                _throwedUnit = list[firstNum];
            }

            while (_throwedUnit == null)
            {
                if (count == 0)
                {
                    _throwedUnit = null;
                    return;
                }

                int find = (lastNum + firstNum) / 2;
                targetPosX = list[find].transform.position.x;

                if (pos.x == targetPosX)
                {
                    _throwedUnit = list[find];
                    break;
                }

                if (pos.x > targetPosX)
                {
                    firstNum = find;
                }
                else if (pos.x < targetPosX)
                {
                    lastNum = find;
                }

                if (lastNum - firstNum <= 1)
                {
                    _throwedUnit = list[lastNum];
                    break;
                }

                loopnum++;
                if (loopnum > 10000)
                {
                    throw new System.Exception("Infinite Loop");
                }
            }

            if (_throwedUnit != null)
            {
                if (_throwedUnit.UnitData.unitType == UnitType.PencilCase)
                {
                    _throwedUnit = null;
                    return;
                }
                if(_throwGauge < _throwedUnit.UnitStat.Return_Weight())
                {
                    _throwedUnit = null;
                    return;
                }
                Vector2[] points = _throwedUnit.CollideData.GetPoint(_throwedUnit.transform.position);
                
                if (CheckPoints(points, pos))
                {
                    _throwedUnit = _throwedUnit.Pull_Unit();

                    if (_throwedUnit == null)
                    {
                        _cameraCommand.SetIsDontMove(true);
                    }
                    else
                    {
                        _throwedUnit.UnitSprite.OrderDraw(-10);
                        _throwedUnit.UnitSticker.OrderDraw(-10);
                        _parabolaBackground.SetActive(true);
                    }

                    _pullTime = 2f;
                    return;
                }
                _throwedUnit = null;
            }
        }

        /// <summary>
        /// 포물선 그리기 & 던지기 취소 조건
        /// </summary>
        /// <param name="pos"></param>
        public void DrawParabola(Vector2 pos)
        {
            UnDrawParabola();
            if (_throwedUnit != null)
            {
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
                    return;
                }

                //방향
                _direction = (Vector2)_throwedUnit.transform.position - pos;
                float dir = Mathf.Atan2(_direction.y, _direction.x);
                float dirx = Mathf.Atan2(_direction.y, -_direction.x);

                //던지는 방향에 따라 포물선만 안 보이게 함
                if (dir < 0)
                {
                    UnDrawParabola();
                    return;
                }

                //화살표
                _arrow.transform.position = _throwedUnit.transform.position;
                _arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);

                //초기 벡터
                _force = Mathf.Clamp(Vector2.Distance(_throwedUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwedUnit.UnitStat.Return_Weight());

                //수평 도달 거리
                float width = Parabola.Caculated_Width(_force, dirx);
                //수평 도달 시간
                float time = Parabola.Caculated_Time(_force, dir, 2);

                SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

                return;
            }
        }

        /// <summary>
        /// 포물선 그리기 해제
        /// </summary>
        private void UnDrawParabola()
        {
            _throwParabolaComponent.UnSetParabolaPos();
        }

        /// <summary>
        /// 라인렌더러의 포물선 위치 줘서 설정
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
        /// 인포인트가 아웃 포인트 안에 있는지 체크
        /// </summary>
        /// <param name="outPoint"></param>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        private bool CheckPoints(Vector2[] box, Vector2 inPoint)
        {
            if (box[0].x - 0.2f > inPoint.x)
            {
                return false;
            }
            if (box[1].x + 0.2f < inPoint.x)
            {
                return false;
            }
            if (box[2].x - 0.2f > inPoint.x)
            {
                return false;
            }
            if (box[3].x + 0.2f < inPoint.x)
            {
                return false;
            }
            if (box[0].y + 0.15f < inPoint.y)
            {
                return false;
            }
            if (box[1].y + 0.15f < inPoint.y)
            {
                return false;
            }
            if (box[2].y - 0.1f > inPoint.y)
            {
                return false;
            }
            if (box[3].y - 0.1f > inPoint.y)
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 던지기 가능한 유닛들의 시각적 효과를 설정한다.
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
        /// 업데이트 딜레이
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
