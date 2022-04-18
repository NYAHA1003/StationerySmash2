using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class ThrowCommand : BattleCommand
    {
        //인스펙터 참조 변수
        [SerializeField]
        private LineRenderer _parabola;
        [SerializeField]
        private Transform _arrow;
        [SerializeField]
        private Image _throwDelayBar; 

        //참조 변수
        private Unit _throwUnit = null;
        private StageData _stageData = null;
        private UnitCommand _unitCommand = null;
        private CameraCommand _cameraCommand = null;

        private List<Vector2> _lineZeroPos;
        private Vector2 _direction;
        private float _force;
        private float _pullTime;
        private float _throwDelay = 0f;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="parabola"></param>
        /// <param name="arrow"></param>
        /// <param name="stageData"></param>
        public void SetInitialization(ref System.Action updateAction, UnitCommand unitCommand, CameraCommand cameraCommand, StageData stageData)
        {
            _unitCommand = unitCommand;
            _cameraCommand = cameraCommand;
            this._stageData = stageData;
            _lineZeroPos = new List<Vector2>(_parabola.positionCount);
            for (int i = 0; i < _parabola.positionCount; i++)
            {
                _lineZeroPos.Add(Vector2.zero);
            }

            updateAction += UpdateThrowDelay;
        }

        /// <summary>
        /// 업데이트 딜레이
        /// </summary>
        public void UpdateThrowDelay()
        {
            if(_throwDelay <= 5f)
            {
                _throwDelay += Time.deltaTime;
                _throwDelayBar.fillAmount = _throwDelay / 5f;
            }
        }

        /// <summary>
        /// 던질 유닛 선택
        /// </summary>
        /// <param name="pos"></param>
        public void PullUnit(Vector2 pos)
        {
            if(_throwDelay < 5f)
            {
                return;
            }

            int firstNum = 0;
            int lastNum = _unitCommand._playerUnitList.Count - 1;
            int loopnum = 0;
            int count = _unitCommand._playerUnitList.Count;
            List<Unit> list = _unitCommand._playerUnitList;
            float targetPosX = 0;
            _throwUnit = null;
            if(pos.x >= list[lastNum].transform.position.x - 0.3f)
            {
                _throwUnit = list[lastNum];
            }
            else if (pos.x <= list[firstNum].transform.position.x )
            {
                _throwUnit = list[firstNum];
            }

            while (_throwUnit == null)
            {
                if (count == 0)
                {
                    _throwUnit = null;
                    return;
                }

                int find = (lastNum + firstNum) / 2;
                targetPosX = list[find].transform.position.x;

                if (pos.x == targetPosX)
                {
                    _throwUnit = list[find];
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
                    _throwUnit = list[lastNum];
                    break;
                }

                loopnum++;
                if (loopnum > 10000)
                {
                    throw new System.Exception("Infinite Loop");
                }
            }

            if (_throwUnit != null)
            {
                if (_throwUnit.UnitData.unitType != UnitType.PencilCase)
                {
                    Debug.Log("유닛 선택 : " + _throwUnit.OrderIndex);
                }
                Vector2[] points = _throwUnit.CollideData.GetPoint(_throwUnit.transform.position);
                
                if (CheckPoints(points, pos))
                {
                    _throwUnit = _throwUnit.Pull_Unit();
                    
                    if (_throwUnit == null)
                    {
                        _cameraCommand.SetCameraIsMove(false);
                    }

                    _pullTime = 2f;
                    return;
                }
                _throwUnit = null;
            }
        }

        /// <summary>
        /// 인포인트가 아웃 포인트 안에 있는지 체크
        /// </summary>
        /// <param name="outPoint"></param>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        public bool CheckPoints(Vector2[] box, Vector2 inPoint)
        {
            if(box[0].x - 0.2f > inPoint.x)
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
        /// 포물선 그리기
        /// </summary>
        /// <param name="pos"></param>
        public void DrawParabola(Vector2 pos)
        {
            UnDrawParabola();
            if (_throwUnit != null)
            {
                //시간이 지나면 취소
                _pullTime -= Time.deltaTime;
                if (_pullTime < 0)
                {
                    _throwUnit = null;
                    UnDrawParabola();
                    return;
                }

                //유닛이 다른 행동을 취하게 되면 취소
                _throwUnit = _throwUnit.Pulling_Unit();
                _cameraCommand.SetCameraIsMove(false);

                if (_throwUnit == null)
                {
                    UnDrawParabola();
                    return;
                }

                //방향
                _direction = (Vector2)_throwUnit.transform.position - pos;
                float dir = Mathf.Atan2(_direction.y, _direction.x);
                float dirx = Mathf.Atan2(_direction.y, -_direction.x);

                //던지는 방향에 따라 포물선만 안 보이게 함
                if (dir < 0)
                {
                    UnDrawParabola();
                    return;
                }

                //화살표
                _arrow.transform.position = _throwUnit.transform.position;
                _arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);

                //초기 벡터
                _force = Mathf.Clamp(Vector2.Distance(_throwUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwUnit.UnitStat.Return_Weight());

                //최고점
                float height = Utill.Parabola.Caculated_Height(_force, dirx);
                //수평 도달 거리
                float width = Utill.Parabola.Caculated_Width(_force, dirx);
                //수평 도달 시간
                float time = Utill.Parabola.Caculated_Time(_force, dir, 2);

                List<Vector2> linePos = SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

                SetParabolaPos(linePos);

                return;
            }
        }

        /// <summary>
        /// 포물선 그리기 해제
        /// </summary>
        public void UnDrawParabola()
        {
            SetParabolaPos(_lineZeroPos);
        }

        /// <summary>
        /// 라인렌더러의 포물선 위치 줘서 설정
        /// </summary>
        /// <param name="linePos"></param>
        private void SetParabolaPos(List<Vector2> linePos)
        {
            for (int i = 0; i < _parabola.positionCount; i++)
            {
                _parabola.SetPosition(i, linePos[i]);
            }
        }

        /// <summary>
        /// 포물선 위치를 반환
        /// </summary>
        /// <param name="count"></param>
        /// <param name="width"></param>
        /// <param name="force"></param>
        /// <param name="dir_rad"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private List<Vector2> SetParabolaPos(int count, float width, float force, float dir_rad, float time)
        {
            List<Vector2> results = new List<Vector2>(count);
            float[] objLerps = new float[count];
            float[] timeLerps = new float[count];
            float interbal = 1f / (count - 1 > 0 ? count - 1 : 1);
            float timeInterbal = time / (count - 1 > 0 ? count - 1 : 1);
            for (int i = 0; i < count; i++)
            {
                objLerps[i] = interbal * i;
                timeLerps[i] = timeInterbal * i;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = Vector3.Lerp((Vector2)_throwUnit.transform.position, new Vector2(_throwUnit.transform.position.x - width, 0), objLerps[i]);
                pos.y = Utill.Parabola.Caculated_TimeToPos(force, dir_rad, timeLerps[i]);

                if (i > 0)
                {
                    if (pos.x >= _stageData.max_Range || pos.x <= -_stageData.max_Range)
                    {
                        pos = results[i - 1];
                    }
                }

                results.Add(pos);
            }

            return results;
        }

        /// <summary>
        /// 유닛 던지기
        /// </summary>
        public void ThrowUnit()
        {
            if (_throwUnit != null)
            {
                _throwUnit.Throw_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _throwUnit = null;
                UnDrawParabola();
                _throwDelay = 0f;
            }
        }
    }

}
