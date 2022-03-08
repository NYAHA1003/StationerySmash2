using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Stationary_Unit_Eff_State : UnitState
{
    protected new Stationary_Unit myUnit;
    protected new Stationary_Unit_Eff_State nextState;  // 다음 상태
    protected UnitData myUnitData;
    private float[] valueList;
    public AtkType statusEffect { get; protected set; }
    public Stationary_Unit_Eff_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] valueList) : base(myTrm, mySprTrm, myUnit)
    {
        this.valueList = valueList;
        this.myUnit = myUnit;
        this.statusEffect = statusEffect;
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
                    nextState = new Stationary_Unit_Sturn_Eff_State(myTrm, mySprTrm, myUnit, statusEffect, valueList);
                    break;
                case AtkType.Ink:
                    nextState = new Stationary_Unit_Ink_Eff_State(myTrm, mySprTrm, myUnit, statusEffect, valueList);
                    break;
                case AtkType.SlowDown:
                    nextState = new Stationary_Unit_SlowDown_Eff_State(myTrm, mySprTrm, myUnit, statusEffect, valueList);
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

    public void Set_EffType(AtkType atkType, params float[] value)
    {
        statusEffect = atkType;
        this.valueList = value;
    }

    public virtual void Set_EffSetting(params float[] value) { }
}
public class Stationary_Unit_Sturn_Eff_State : Stationary_Unit_Eff_State
{
    private float stunTime;

    public Stationary_Unit_Sturn_Eff_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] stunTime) : base(myTrm, mySprTrm, myUnit, statusEffect, stunTime)
    {
        this.stunTime = stunTime[0];
    }
    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
        myUnit.unitState.nextState = StateChangeManager.Set_Wait(myUnit.unitState,stunTime);

        base.Enter();
    }

    public override void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }
        nextState = new Stationary_Unit_Eff_State(myTrm, mySprTrm, myUnit, AtkType.Normal, null);
        curEvent = eEvent.EXIT;
    }

    public override void Set_EffSetting(params float[] value)
    {
        if (stunTime < value[0])
        {
            stunTime = value[0];
            stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));
        }
    }
}
public class Stationary_Unit_Ink_Eff_State : Stationary_Unit_Eff_State
{
    private float inkTime = 0;
    private float damageSubtractPercent = 0;
    private float accuracySubtractPercent = 0;
    private float range;

    public Stationary_Unit_Ink_Eff_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] value) : base(myTrm, mySprTrm, myUnit, statusEffect, value)
    {
        Set_EffSetting(value);
    }
    public override void Enter()
    {
        mySprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        myUnit.damagePercent -= (int)damageSubtractPercent;
        myUnit.accuracyPercent -= (int)accuracySubtractPercent;

        if(myUnit.eTeam == TeamType.MyTeam)
        {
            //Check_InkRangeMyTeam(myUnit.battleManager.unit_MyDatasTemp);
        }
        else
        {
            //Check_InkRangeMyTeam(myUnit.battleManager.unit_EnemyDatasTemp);
        }

        base.Enter();
    }

    //private void Check_InkRangeMyTeam(List<Unit> list)
    //{
    //    Stationary_Unit target_Unit = null;
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        target_Unit = list[i].GetComponent<Stationary_Unit>();
            
    //        if (target_Unit == null) 
    //            continue;

    //        if (target_Unit == myUnit)
    //            continue;

    //        //거리 비교해서 잉크 적용
    //        if (Mathf.Abs(target_Unit.transform.position.x - originPosX) < range)
    //        {
    //            target_Unit.Add_StatusEffect(AtkType.Ink, inkTime, damageSubtractPercent, accuracySubtractPercent, originPosX, range);
    //        }
    //    }
    //}

    public override void Update()
    {
        if (inkTime > 0)
        {
            inkTime -= Time.deltaTime;
            return;
        }


        nextState = new Stationary_Unit_Eff_State(myTrm, mySprTrm, myUnit, AtkType.Normal, null);
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.damagePercent += (int)damageSubtractPercent;
        myUnit.accuracyPercent += (int)accuracySubtractPercent;
        mySprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void Set_EffSetting(params float[] value)
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

    public Stationary_Unit_SlowDown_Eff_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, AtkType statusEffect, params float[] value) : base(myTrm, mySprTrm, myUnit, statusEffect, value)
    {
        Set_EffSetting(value);
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


        nextState = new Stationary_Unit_Eff_State(myTrm, mySprTrm, myUnit, AtkType.Normal, null);
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.moveSpeedPercent += (int)moveSpeedSubtractPercent;
        myUnit.attackSpeedPercent += (int)attackSpeedSubtractPercent;

        base.Exit();
    }

    public override void Set_EffSetting(params float[] value)
    {
        slowDownTime = value[0];
        moveSpeedSubtractPercent = value[1];
        attackSpeedSubtractPercent = value[2];
    }
}
