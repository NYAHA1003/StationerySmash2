using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class SlowDownEffState : EffState
{
    private float slowDownTime = 0; //슬로우 다운 유지 시간
    private float moveSpeedSubtractPercent = 0; //이동속도 퍼센트가 얼마나 느려질 것인가
    private float attackSpeedSubtractPercent = 0; //공격속도 퍼센트가 얼마나 느려질 것인가

    public override void Enter()
    {
        _myUnit.UnitStat.IncreaseMoveSpeedPercent(-(int)moveSpeedSubtractPercent);
        _myUnit.UnitStat.IncreaseAttackSpeedPercent(-(int)attackSpeedSubtractPercent);
        _effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Slow, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), slowDownTime, Trm));

        base.Enter();
    }

    public override void Update()
    {
        SlowDownTimer();
    }

    public override void Exit()
    {
        _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);
        _myUnit.UnitStat.IncreaseAttackSpeedPercent((int)attackSpeedSubtractPercent);
        DeleteEffectObject();
        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        slowDownTime = value[0];
        moveSpeedSubtractPercent = value[1];
        attackSpeedSubtractPercent = value[2];
    }

    /// <summary>
    /// 슬로우다운 유지 시간
    /// </summary>
    private void SlowDownTimer()
    {
        if (slowDownTime > 0)
        {
            slowDownTime -= Time.deltaTime;
            return;
        }
        _curEvent = eEvent.EXIT;
    }
}
