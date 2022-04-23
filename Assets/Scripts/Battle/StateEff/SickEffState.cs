using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class SickEffState : EffState
{
    private float _sickTime = 0; //잉크 지속시간
    private float _moveSpeedSubtractPercent = 0; //이동 반대방량
    private int _originpercent = 0;

    public override void Enter()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        _originpercent = _myUnit.UnitStat.MoveSpeedPercent;
        _myUnit.UnitStat.IncreaseMoveSpeedPercent(-_originpercent * 2);

        

        base.Enter();
    }
    public override void Update()
    {
        SickTimer();
    }

    public override void Exit()
    {
        _myUnit.UnitStat.IncreaseMoveSpeedPercent(_originpercent * 2);
        SprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        _sickTime = value[0];
    }

    /// <summary>
    /// 잉크 효과 지속시간
    /// </summary>
    private void SickTimer()
    {
        if (_sickTime > 0)
        {
            _sickTime -= Time.deltaTime;
            return;
        }
        _curEvent = eEvent.EXIT;
    }
}
