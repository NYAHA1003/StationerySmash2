using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class Eff_State
{
    public eState curState { get; protected set; }
    public eEvent curEvent;
    public Transform myTrm { get; protected set; }
    public Transform mySprTrm { get; protected set; }
    public AtkType statusEffect { get; protected set; }

    public BattleManager battleManager;
    protected Stationary_Unit myUnit;
    protected Eff_State nextState;
    protected UnitData myUnitData;
    protected float[] valueList;

    public Eff_State(BattleManager battleManager, Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] valueList)
    {
        this.battleManager = battleManager;
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
        this.statusEffect = statusEffect;
        this.valueList = valueList;
    }


    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    public Eff_State Process()
    {
        if (curEvent.Equals(eEvent.ENTER))
        {
            Enter();
        }
        if (curEvent.Equals(eEvent.UPDATE))
        {
            Update();
        }
        if (curEvent.Equals(eEvent.EXIT))
        {
            Exit();
            return nextState;
        }

        return this;
    }
    public void Set_EffType(AtkType atkType, params float[] value)
    {
        statusEffect = atkType;
        this.valueList = value;
    }

    public abstract void Set_EffValue(params float[] value);

}

public class Stationary_Unit_Eff_State : Eff_State
{
    public Stationary_Unit_Eff_State(BattleManager battleManager, Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] valueList) : base(battleManager, myTrm, mySprTrm, myUnit, statusEffect, valueList)
    {
        this.valueList = valueList;
        this.myUnit = myUnit;
    }
    public override void Update()
    {
        if(statusEffect != AtkType.Normal)
        {
            switch (statusEffect)
            {
                default:
                case AtkType.Normal:
                    break;
                case AtkType.Stun:
                    nextState = new Stationary_Unit_Sturn_Eff_State(battleManager, myTrm, mySprTrm, myUnit, statusEffect, valueList);
                    break;
                case AtkType.Ink:
                    nextState = new Stationary_Unit_Ink_Eff_State(battleManager, myTrm, mySprTrm, myUnit, statusEffect, valueList);
                    break;
                case AtkType.SlowDown:
                    nextState = new Stationary_Unit_SlowDown_Eff_State(battleManager, myTrm, mySprTrm, myUnit, statusEffect, valueList);
                    break;
            }
            curEvent = eEvent.EXIT;
        }
    }


    public override void Set_EffValue(params float[] value)
    {

    }

}
public class Stationary_Unit_Sturn_Eff_State : Eff_State
{
    private float stunTime;

    public Stationary_Unit_Sturn_Eff_State(BattleManager battleManager, Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] stunTime) : base(battleManager, myTrm, mySprTrm, myUnit, statusEffect, stunTime)
    {
        this.stunTime = stunTime[0];
    }
    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
        myUnit.Set_IsDontThrow(true);
        myUnit.unitState.stateChange.Return_Wait(stunTime);
        myUnit.unitState.stateChange.Set_WaitExtraTime(stunTime);
        battleManager.battle_Effect.Set_Effect(EffectType.Stun, new EffData(new Vector2(myTrm.position.x, myTrm.position.y + 0.1f), stunTime, myTrm));

        base.Enter();
    }

    public override void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            myUnit.unitState.stateChange.Set_WaitExtraTime(stunTime);
            return;
        }
        myUnit.Set_IsDontThrow(false);
        nextState = new Stationary_Unit_Eff_State(battleManager, myTrm, mySprTrm, myUnit, AtkType.Normal, null);
        curEvent = eEvent.EXIT;
    }

    public override void Set_EffValue(params float[] value)
    {
        if (stunTime < value[0])
        {
            stunTime = value[0];
            stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
        }
    }
}
public class Stationary_Unit_Ink_Eff_State : Eff_State
{
    private float inkTime = 0;
    private float damageSubtractPercent = 0;
    private float accuracySubtractPercent = 0;
    private float range;

    public Stationary_Unit_Ink_Eff_State(BattleManager battleManager, Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] value) : base(battleManager, myTrm, mySprTrm, myUnit, statusEffect, value)
    {
        Set_EffValue(value);
    }
    public override void Enter()
    {
        mySprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        myUnit.damagePercent -= (int)damageSubtractPercent;
        myUnit.accuracyPercent -= (int)accuracySubtractPercent;

        base.Enter();
    }
    public override void Update()
    {
        if (inkTime > 0)
        {
            inkTime -= Time.deltaTime;
            return;
        }

        nextState = new Stationary_Unit_Eff_State(battleManager, myTrm, mySprTrm, myUnit, AtkType.Normal, null);
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.damagePercent += (int)damageSubtractPercent;
        myUnit.accuracyPercent += (int)accuracySubtractPercent;
        mySprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void Set_EffValue(params float[] value)
    {
        inkTime = value[0];
        damageSubtractPercent = value[1];
        accuracySubtractPercent = value[2];
        range = value[3];
    }
}

public class Stationary_Unit_SlowDown_Eff_State : Stationary_Unit_Eff_State
{
    private float slowDownTime = 0;
    private float moveSpeedSubtractPercent = 0;
    private float attackSpeedSubtractPercent = 0;

    public Stationary_Unit_SlowDown_Eff_State(BattleManager battleManager, Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] value) : base(battleManager, myTrm, mySprTrm, myUnit, statusEffect, value)
    {
        Set_EffValue(value);
    }
    public override void Enter()
    {
        myUnit.moveSpeedPercent -= (int)moveSpeedSubtractPercent;
        myUnit.attackSpeedPercent -= (int)attackSpeedSubtractPercent;

        base.Enter();
    }

    public override void Update()
    {
        if (slowDownTime > 0)
        {
            slowDownTime -= Time.deltaTime;
            return;
        }


        nextState = new Stationary_Unit_Eff_State(battleManager, myTrm, mySprTrm, myUnit, AtkType.Normal, null);
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.moveSpeedPercent += (int)moveSpeedSubtractPercent;
        myUnit.attackSpeedPercent += (int)attackSpeedSubtractPercent;

        base.Exit();
    }

    public override void Set_EffValue(params float[] value)
    {
        slowDownTime = value[0];
        moveSpeedSubtractPercent = value[1];
        attackSpeedSubtractPercent = value[2];
    }
}
