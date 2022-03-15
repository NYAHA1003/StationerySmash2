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
    public AtkType statusType { get; protected set; }

    public BattleManager battleManager;
    protected Unit myUnit;
    protected UnitData myUnitData;
    protected float[] valueList;

    public Eff_State()
    {
    }

    public void Set_StateEff(Transform myTrm, Transform mySprTrm, Unit myUnit, AtkType statusEffect, params float[] valueList)
    {
        curEvent = eEvent.ENTER;
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
        Set_EffType(statusEffect, valueList);
        Set_EffValue(valueList);
        this.battleManager = myUnit.battleManager;
    }
    public void Reset_StateEff(Transform myTrm, Transform mySprTrm, Unit myUnit, AtkType statusEffect, params float[] valueList)
    {
        curEvent = eEvent.ENTER;
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
        Set_EffType(statusEffect, valueList);
        Set_EffValue(valueList);
        this.battleManager = myUnit.battleManager;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() 
    {
        Delete_StatusEffect();
        curEvent = eEvent.EXIT; 
    }

    public void Process()
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
        }
    }

    public void Set_EffType(AtkType atkType, params float[] value)
    {
        statusType = atkType;
        this.valueList = value;
    }

    public abstract void Set_EffValue(params float[] value);

    /// <summary>
    /// 효과 삭제
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="eff_State"></param>
    public void Delete_StatusEffect()
    {
        myUnit.statEffList.Remove(this);

        switch (statusType)
        {
            case AtkType.Normal:
                break;
            case AtkType.Stun:
                Battle_Unit.AddEff((Sturn_Eff_State)this);
                break;
            case AtkType.Ink:
                Battle_Unit.AddEff((Ink_Eff_State)this);
                break;
            case AtkType.SlowDown:
                Battle_Unit.AddEff((SlowDown_Eff_State)this);
                break;
        }

    }
}
public class Sturn_Eff_State : Eff_State
{
    private float stunTime = 0.0f;

    public Sturn_Eff_State() : base()
    {
    }
    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
        myUnit.Set_IsDontThrow(true);
        myUnit.unitState.stateChange.Set_Wait(stunTime);
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
public class Ink_Eff_State : Eff_State
{
    private float inkTime = 0;
    private float damageSubtractPercent = 0;
    private float accuracySubtractPercent = 0;

    public Ink_Eff_State() : base()
    {
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
    }
}
public class SlowDown_Eff_State : Eff_State
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

        base.Exit();
    }

    public override void Set_EffValue(params float[] value)
    {
        slowDownTime = value[0];
        moveSpeedSubtractPercent = value[1];
        attackSpeedSubtractPercent = value[2];
    }
}
