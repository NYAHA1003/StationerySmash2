using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class BattleThrow : BattleCommand
{
    private Unit _throwUnit = null;
    private LineRenderer _parabola;
    private Transform _arrow;
    private StageData _stageData;

    private List<Vector2> _lineZeroPos;
    private Vector2 _direction;
    private float _force;
    private float _pullTime;


    public BattleThrow(BattleManager battleManager, LineRenderer parabola, Transform arrow, StageData stageData) : base(battleManager)
    {
        this._parabola = parabola;
        this._stageData = stageData;
        this._arrow = arrow;
        _lineZeroPos = new List<Vector2>(parabola.positionCount);
        for(int i = 0; i < parabola.positionCount; i++)
        {
            _lineZeroPos.Add(Vector2.zero);
        }
    }

    public void PullUnit(Vector2 pos)
    {
        float targetRange = float.MaxValue;
        for (int i = 0; i < battleManager._myUnitDatasTemp.Count; i++)
        {
            if (Vector2.Distance(pos, battleManager._myUnitDatasTemp[i].transform.position) < targetRange)
            {
                _throwUnit = battleManager._myUnitDatasTemp[i];
                targetRange = Vector2.Distance(pos, _throwUnit.transform.position);
            }
        }

        if (_throwUnit != null)
        {
            if (Vector2.Distance(pos, _throwUnit.transform.position) < 0.1f)
            {
                _throwUnit = _throwUnit.Pull_Unit();
                if(_throwUnit == null)
                {
                    battleManager.BattleCamera.SetCameraIsMove(false);
                }
                _pullTime = 2f;
                return;
            }
            _throwUnit = null;
        }
    }

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
            battleManager.BattleCamera.SetCameraIsMove(false);

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
            if(dir < 0)
            {
                UnDrawParabola();
                return;
            }

            //화살표
            _arrow.transform.position = _throwUnit.transform.position;
            _arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);
            
            //초기 벡터
            _force = Mathf.Clamp(Vector2.Distance(_throwUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwUnit.weight);

            //최고점
            float height = Utill.Parabola.Caculated_Height(_force, dirx);
            //수평 도달 거리
            float width = Utill.Parabola.Caculated_Width(_force, dirx) ;
            //수평 도달 시간
            float time = Utill.Parabola.Caculated_Time(_force, dir, 2);
            
            List<Vector2> linePos = SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

            SetParabolaPos(linePos);

            return;
        }
    }

    public void UnDrawParabola()
    {
        SetParabolaPos(_lineZeroPos);
    }

    private void SetParabolaPos(List<Vector2> linePos)
    {
        for (int i = 0; i < _parabola.positionCount; i++)
        {
            _parabola.SetPosition(i, linePos[i]);
        }
    }


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

        for(int i = 0; i < count; i ++)
        {
            Vector3 pos = Vector3.Lerp((Vector2)_throwUnit.transform.position, new Vector2(_throwUnit.transform.position.x - width, 0), objLerps[i]);
            pos.y = Utill.Parabola.Caculated_TimeToPos(force, dir_rad, timeLerps[i]);
            
            if(i > 0)
            {
                if(pos.x >= _stageData.max_Range || pos.x <= -_stageData.max_Range)
                {
                    pos = results[i - 1];
                }
            }

            results.Add(pos);
        }

        return results;
    }

    public void ThrowUnit()
    {
        if (_throwUnit != null)
        {
            _throwUnit.Throw_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _throwUnit = null;
            UnDrawParabola();
        }
    }
}
