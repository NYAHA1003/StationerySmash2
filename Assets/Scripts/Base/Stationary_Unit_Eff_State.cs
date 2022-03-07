using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Stationary_Unit_Eff_State : UnitState
{
    protected new Stationary_Unit myUnit;
    protected new Stationary_Unit_Eff_State nextState;  // 다음 상태
    protected UnitData myUnitData;
    private float value;
    public AtkType statusEffect { get; protected set; }
    public Stationary_Unit_Eff_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, float value) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.NONE;
        this.value = value;
        this.myUnit = myUnit;
        this.statusEffect = statusEffect;
    }
    public override void Update()
    {
        if(statusEffect != AtkType.Normal)
        {
            switch(statusEffect)
            {
                default:
                case AtkType.Normal:
                    break;
                case AtkType.Stun:
                    nextState = new Stationary_Unit_Sturn_Eff_State(myTrm, mySprTrm, myUnit, statusEffect, value);
                    break;
            }
            curEvent = eEvent.EXIT;
        }
    }

    public new Stationary_Unit_Eff_State Process()
    {
        if (curEvent == eEvent.ENTER)
        {
            Enter();
        }
        if (curEvent == eEvent.UPDATE)
        {
            Update();
        }
        if (curEvent == eEvent.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    public void Set_EffType(AtkType atkType, float value)
    {
        statusEffect = atkType;
        this.value = value;
    }

    public virtual void Set_EffSetting(float value) { }
}
public class Stationary_Unit_Sturn_Eff_State : Stationary_Unit_Eff_State
{
    private float stunTime;

    public Stationary_Unit_Sturn_Eff_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, float stunTime) : base(myTrm, mySprTrm, myUnit, statusEffect, stunTime)
    {
        this.stunTime = stunTime;
    }
    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
        Debug.Log("스턴: " + stunTime);
        myUnit.unitState.Set_Wait(stunTime);

        base.Enter();
    }

    public override void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }
        nextState = new Stationary_Unit_Eff_State(myTrm, mySprTrm, myUnit, AtkType.Normal, 0);
        curEvent = eEvent.EXIT;
    }

    public override void Set_EffSetting(float time)
    {
        if(stunTime < time)
        {
            stunTime = time;
            stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
            Debug.Log("스턴: " + stunTime);
        }
    }
}