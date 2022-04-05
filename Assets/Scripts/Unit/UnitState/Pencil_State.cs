using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilStateManager : IStateManager
{
    private Pencil_Idle_State IdleState;
    private Pencil_Attack_State AttackState;
    private Pencil_Damaged_State DamagedState;
    private Pencil_Throw_State ThrowState;
    private Pencil_Die_State DieState;
    private Pencil_Move_State MoveState;
    private Pencil_Wait_State WaitState;
    private UnitState cur_unitState;
    public StageData _stageData { get; private set; }

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
        //스테이트들을 설정한다
        IdleState = new Pencil_Idle_State();
        WaitState = new Pencil_Wait_State();
        MoveState = new Pencil_Move_State();
        AttackState = new Pencil_Attack_State();
        DamagedState = new Pencil_Damaged_State();
        DieState = new Pencil_Die_State();
        ThrowState = new Pencil_Throw_State();

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

        Set_WaitExtraTime(0);
        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Attack(Unit targetUnit)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        AttackState.Set_Target(targetUnit);
        cur_unitState._nextState = AttackState;
        AttackState.Reset_State();
        Reset_CurrentUnitState(AttackState);
    }

    public void Set_Damaged(AtkData atkData)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        DamagedState.Set_AtkData(atkData);
        cur_unitState._nextState = DamagedState;
        DamagedState.Reset_State();
        Reset_CurrentUnitState(DamagedState);
    }

    public void Set_Die()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState._nextState = DieState;
        DieState.Reset_State();
        Reset_CurrentUnitState(DieState);
    }

    public void Set_Idle()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState._nextState = IdleState;
        IdleState.Reset_State();
        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Move()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState._nextState = MoveState;
        MoveState.Reset_State();
        Reset_CurrentUnitState(MoveState);
    }

    public void Set_Throw()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState._nextState = ThrowState;
        ThrowState.Reset_State();
        Reset_CurrentUnitState(ThrowState);
    }

    public void Set_Wait(float time)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        WaitState.Set_Time(time);
        WaitState.Set_ExtraTime(Wait_extraTime);
        cur_unitState._nextState = WaitState;
        WaitState.Reset_State();
        Reset_CurrentUnitState(WaitState);
    }


    public void Set_WaitExtraTime(float extraTime)
    {
        this.Wait_extraTime = extraTime;
    }

    public void Set_ThrowPos(Vector2 pos)
    {
        this.ThrowState.Set_ThrowPos(pos);
    }

    public void SetStageData(StageData stageData)
    {
        _stageData = stageData;
    }

    public StageData GetStageData()
    {
        return _stageData;
    }
}

public class Pencil_Idle_State : UnitIdleState
{
}

public class Pencil_Wait_State : UnitWaitState
{
}

public class Pencil_Move_State : UnitMoveState
{
}

public class Pencil_Attack_State : UnitAttackState
{
}

public class Pencil_Damaged_State : UnitDamagedState
{
}

public class Pencil_Die_State : UnitDieState
{
}

public class Pencil_Throw_State : UnitThrowState
{

}

