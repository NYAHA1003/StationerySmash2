using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class BlindEffState : EffState
{
    private float _blindTime = 0; //블라인드 지속시간
    private float _rangeSubtractPercent = 50; //사정거리 퍼센트가 얼마나 줄어들것인가

    public override void Enter()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        _myUnit.UnitStat.IncreaseRangePercent(-(int)_rangeSubtractPercent);

        base.Enter();
    }
    public override void Update()
    {
        InkTimer();
    }

    public override void Exit()
    {
        _myUnit.UnitStat.IncreaseRangePercent((int)_rangeSubtractPercent);
        SprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        _blindTime = value[0];
    }

    /// <summary>
    /// 잉크 효과 지속시간
    /// </summary>
    private void InkTimer()
    {
        if (_blindTime > 0)
        {
            _blindTime -= Time.deltaTime;
            return;
        }
        _curEvent = eEvent.EXIT;
    }
}
