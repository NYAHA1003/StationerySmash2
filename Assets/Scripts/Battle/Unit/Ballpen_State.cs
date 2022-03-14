using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class BallpenStateManager : IStateManager
{
    private Ballpen_Idle_State IdleState;
    private Ballpen_Attack_State AttackState;
    private Ballpen_Damaged_State DamagedState;
    private Ballpen_Throw_State ThrowState;
    private Ballpen_Die_State DieState;
    private Ballpen_Move_State MoveState;
    private Ballpen_Wait_State WaitState;
    private UnitState cur_unitState;

    private float Wait_extraTime = 0;

    public void Reset_CurrentUnitState(UnitState unitState)
    {
        cur_unitState = unitState;
    }
    public UnitState Return_CurrentUnitState()
    {
        return cur_unitState;
    }

    public void Set_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        Debug.Log("생성");

        //스테이트들을 설정한다
        IdleState = new    Ballpen_Idle_State(myTrm, mySprTrm, myUnit);
        WaitState = new    Ballpen_Wait_State(myTrm, mySprTrm, myUnit);
        MoveState = new    Ballpen_Move_State(myTrm, mySprTrm, myUnit);
        AttackState = new  Ballpen_Attack_State(myTrm, mySprTrm, myUnit);
        DamagedState = new Ballpen_Damaged_State(myTrm, mySprTrm, myUnit);
        DieState = new     Ballpen_Die_State(myTrm, mySprTrm, myUnit);
        ThrowState = new Ballpen_Throw_State(myTrm, mySprTrm, myUnit);

        Reset_CurrentUnitState(IdleState);

        IdleState.Set_StateChange(this);
        WaitState.Set_StateChange(this);
        MoveState.Set_StateChange(this);
        AttackState.Set_StateChange(this);
        DamagedState.Set_StateChange(this);
        DieState.Set_StateChange(this);
        ThrowState.Set_StateChange(this);
    }
    public void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        Debug.Log("재활용");

        IdleState.Change_Trm(myTrm, mySprTrm, myUnit);
        WaitState.Change_Trm(myTrm, mySprTrm, myUnit);
        MoveState.Change_Trm(myTrm, mySprTrm, myUnit);
        AttackState.Change_Trm(myTrm, mySprTrm, myUnit);
        DamagedState.Change_Trm(myTrm, mySprTrm, myUnit);
        DieState.Change_Trm(myTrm, mySprTrm, myUnit);
        ThrowState.Change_Trm(myTrm, mySprTrm, myUnit);

        IdleState.Reset_State();
        WaitState.Reset_State();
        MoveState.Reset_State();
        AttackState.Reset_State();
        DamagedState.Reset_State();
        DieState.Reset_State();
        ThrowState.Reset_State();

        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Attack(Unit targetUnit)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        AttackState.Set_Target(targetUnit);
        cur_unitState.nextState = AttackState;
        AttackState.Reset_State();
        Reset_CurrentUnitState(AttackState);
    }

    public void Set_Damaged(AtkData atkData)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        DamagedState.Set_AtkData(atkData);
        cur_unitState.nextState = DamagedState;
        DamagedState.Reset_State();
        Reset_CurrentUnitState(DamagedState);
    }

    public void Set_Die()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = DieState;
        DieState.Reset_State();
        Reset_CurrentUnitState(DieState);
    }

    public void Set_Idle()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = IdleState;
        IdleState.Reset_State();
        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Move()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = MoveState;
        MoveState.Reset_State();
        Reset_CurrentUnitState(MoveState);
    }

    public void Set_Throw()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = ThrowState;
        ThrowState.Reset_State();
        Reset_CurrentUnitState(ThrowState);
    }

    public void Set_Wait(float time)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        WaitState.Set_Time(time);
        WaitState.Set_ExtraTime(Wait_extraTime);
        cur_unitState.nextState = WaitState;
        WaitState.Reset_State();
        Reset_CurrentUnitState(WaitState);
    }


    public void Set_WaitExtraTime(float extraTime)
    {
        this.Wait_extraTime = extraTime;
    }

}

public class Ballpen_Idle_State : Pencil_Idle_State
{
    public Ballpen_Idle_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Wait_State : Pencil_Wait_State
{
    public Ballpen_Wait_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {

    }
}

public class Ballpen_Move_State : Pencil_Move_State
{
    public Ballpen_Move_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Attack_State : Pencil_Attack_State
{
    public Ballpen_Attack_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Damaged_State : Pencil_Damaged_State
{
    public Ballpen_Damaged_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Die_State : Pencil_Die_State
{
    public Ballpen_Die_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

}

public class Ballpen_Throw_State : Pencil_Throw_State
{
    public Ballpen_Throw_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

}

