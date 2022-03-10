using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class BallPenStateChange : IStateChange
{
    private BallPen_Idle_State IdleState;
    private BallPen_Attack_State AttackState;
    private BallPen_Damaged_State DamagedState;
    private BallPen_Throw_State ThrowState;
    private BallPen_Die_State DieState;
    private BallPen_Move_State MoveState;
    private BallPen_Wait_State WaitState;
    private Stationary_UnitState unit;

    public void Set_State(Stationary_UnitState unit)
    {
        this.unit = unit;
        IdleState = new    BallPen_Idle_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        WaitState = new    BallPen_Wait_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        MoveState = new    BallPen_Move_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        AttackState = new  BallPen_Attack_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        DamagedState = new BallPen_Damaged_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        DieState = new     BallPen_Die_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        ThrowState = new   BallPen_Throw_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);

        IdleState.Set_StateChange(this);
        WaitState.Set_StateChange(this);
        MoveState.Set_StateChange(this);
        AttackState.Set_StateChange(this);
        DamagedState.Set_StateChange(this);
        DieState.Set_StateChange(this);
        ThrowState.Set_StateChange(this);

    }
    public void Return_Attack(Unit targetUnit)
    {
        unit.Set_Event(eEvent.EXIT);
        AttackState.Set_Target(targetUnit);
        unit.nextState = AttackState;
        AttackState.Reset_State();
        unit = AttackState;
    }

    public void Return_Damaged(AtkData atkData)
    {
        unit.Set_Event(eEvent.EXIT);
        DamagedState.Set_AtkData(atkData);
        unit.nextState = DamagedState;
        DamagedState.Reset_State();
        unit = DamagedState;
    }

    public void Return_Die()
    {
        unit.Set_Event(eEvent.EXIT);
        unit.nextState = DieState;
        DieState.Reset_State();
        unit = DieState;
    }

    public void Return_Idle()
    {
        unit.Set_Event(eEvent.EXIT);
        unit.nextState = IdleState;
        IdleState.Reset_State();
        unit = IdleState;
    }

    public void Return_Move()
    {
        unit.Set_Event(eEvent.EXIT);
        unit.nextState = MoveState;
        MoveState.Reset_State();
        unit = MoveState;
    }

    public void Return_Throw()
    {
        unit.Set_Event(eEvent.EXIT);
        unit.nextState = ThrowState;
        ThrowState.Reset_State();
        unit = ThrowState;
    }

    public void Return_Wait(float time)
    {
        unit.Set_Event(eEvent.EXIT);
        WaitState.Set_Time(time);
        unit.nextState = WaitState;
        WaitState.Reset_State();
        unit = WaitState;
    }
}
public class BallPen_Idle_State : Pencil_Idle_State
{
    public BallPen_Idle_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Wait_State : Pencil_Wait_State
{
    public BallPen_Wait_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Move_State : Pencil_Move_State
{
    public BallPen_Move_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Damaged_State : Pencil_Damaged_State
{
    public BallPen_Damaged_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Attack_State : Pencil_Attack_State
{
    public BallPen_Attack_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Die_State : Pencil_Die_State
{
    public BallPen_Die_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Throw_State : Pencil_Throw_State
{
    public BallPen_Throw_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        this.originAtkType = myUnit.unitData.atkType;
        this.originValue = myUnit.unitData.unitablityData;
    }

    protected override void Run_ThrowAttack(Unit targetUnit)
    {
        float dir = Vector2.Angle((Vector2)myTrm.position, (Vector2)targetUnit.transform.position);
        float extraKnockBack = (targetUnit.weight - myUnit.Return_Weight() * (float)targetUnit.hp / targetUnit.maxhp) * 0.025f;
        AtkData atkData = new AtkData(myUnit, 0, 0, 0, 0, true, 0, originAtkType, originValue);

        if(myUnit.eTeam.Equals(TeamType.MyTeam))
        {
            IntAttack(myUnit.battleManager.unit_EnemyDatasTemp);
        }
        else
        {
            IntAttack(myUnit.battleManager.unit_MyDatasTemp);
        }

        atkData.Reset_Damage(100 + (myUnit.weight > targetUnit.weight ? (Mathf.RoundToInt((float)myUnit.weight - targetUnit.weight) / 2) : Mathf.RoundToInt((float)(targetUnit.weight - myUnit.weight) / 5)));


        //무게가 더 클 경우
        if (myUnit.weight > targetUnit.weight)
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 더 작을 경우
        if (myUnit.weight < targetUnit.weight)
        {
            atkData.Reset_Kncockback(0, 0, 0, false);
            atkData.Reset_Type(AtkType.Normal);
            atkData.Reset_Value(0);
            targetUnit.Run_Damaged(atkData);

            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 같을 경우
        if (myUnit.weight.Equals(targetUnit.weight))
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);


            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);

            return;
        }
    }

    private void IntAttack(List<Unit> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(Vector2.Distance(myTrm.position, list[i].transform.position) < originValue[3])
            {
                list[i].Add_StatusEffect(originAtkType, originValue);
            }
        }
    }
}