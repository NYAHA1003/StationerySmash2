using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class SlowDown_Eff_State : EffState
{
    private float slowDownTime = 0;
    private float moveSpeedSubtractPercent = 0;
    private float attackSpeedSubtractPercent = 0;

    public SlowDown_Eff_State() : base()
    {
    }
    public override void Enter()
    {
        myUnit.moveSpeedPercent -= (int)moveSpeedSubtractPercent;
        myUnit.attackSpeedPercent -= (int)attackSpeedSubtractPercent;
        effectObj = battleManager.CommandEffect.SetEffect(EffectType.Slow, new EffData(new Vector2(myTrm.position.x, myTrm.position.y + 0.1f), slowDownTime, myTrm));

        base.Enter();
    }

    public override void Update()
    {
        if (slowDownTime > 0)
        {
            slowDownTime -= Time.deltaTime;
            return;
        }
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.moveSpeedPercent += (int)moveSpeedSubtractPercent;
        myUnit.attackSpeedPercent += (int)attackSpeedSubtractPercent;

        if (effectObj != null)
        {
            effectObj.Delete_Effect();
            effectObj = null;
        }

        base.Exit();
    }

    public override void Set_EffValue(params float[] value)
    {
        slowDownTime = value[0];
        moveSpeedSubtractPercent = value[1];
        attackSpeedSubtractPercent = value[2];
    }
}
