using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Sturn_Eff_State : EffState
{
    private float stunTime = 0.0f;

    public Sturn_Eff_State() : base()
    {
    }
    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 70)) - 1));
        myUnit.Set_IsDontThrow(true);
        myUnit.unitState._stateManager.Set_Wait(stunTime);
        myUnit.unitState._stateManager.Set_WaitExtraTime(stunTime);
        effectObj = battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(myTrm.position.x, myTrm.position.y + 0.1f), stunTime, myTrm));

        base.Enter();
    }

    public override void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            myUnit.unitState._stateManager.Set_WaitExtraTime(stunTime);
            return;
        }
        myUnit.Set_IsDontThrow(false);
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        if (effectObj != null)
        {
            effectObj.Delete_Effect();
            effectObj = null;
        }
        base.Exit();
    }

    public override void Set_EffValue(params float[] value)
    {
        if (stunTime < value[0])
        {
            stunTime = value[0];
            stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 70)) - 1));
        }
    }
}