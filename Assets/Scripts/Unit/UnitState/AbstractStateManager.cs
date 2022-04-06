using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class  AbstractStateManager
{
    public StageData _stageData { get; private set; } = null;

    protected AbstractIdleState _idleState = null;
    protected AbstractAttackState _attackState = null;
    protected AbstractDamagedState _damagedState = null;
    protected AbstractThrowState _throwState = null;
    protected AbstractDieState _dieState = null;
    protected AbstractMoveState _moveState = null;
    protected AbstractWaitState _waitState = null;
    protected AbstractUnitState _currrentState = null;
    protected float _waitExtraTime = 0;

    public void Reset_CurrentUnitState(AbstractUnitState unitState)
    {
        _currrentState = unitState;
    }
    public AbstractUnitState Return_CurrentUnitState()
    {
        return _currrentState;
    }

    public abstract void Set_State();
    public virtual void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        _idleState.Change_Trm(myTrm, mySprTrm, myUnit);
        _waitState.Change_Trm(myTrm, mySprTrm, myUnit);
        _moveState.Change_Trm(myTrm, mySprTrm, myUnit);
        _attackState.Change_Trm(myTrm, mySprTrm, myUnit);
        _damagedState.Change_Trm(myTrm, mySprTrm, myUnit);
        _dieState.Change_Trm(myTrm, mySprTrm, myUnit);
        _throwState.Change_Trm(myTrm, mySprTrm, myUnit);

        _idleState.Reset_State();
        _waitState.Reset_State();
        _moveState.Reset_State();
        _attackState.Reset_State();
        _damagedState.Reset_State();
        _dieState.Reset_State();
        _throwState.Reset_State();

        Set_WaitExtraTime(0);
        Reset_CurrentUnitState(_idleState);
    }

    public void Set_Attack(Unit targetUnit)
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _attackState.Set_Target(targetUnit);
        _currrentState._nextState = _attackState;
        _attackState.Reset_State();
        Reset_CurrentUnitState(_attackState);
    }

    public void Set_Damaged(AtkData atkData)
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _damagedState.Set_AtkData(atkData);
        _currrentState._nextState = _damagedState;
        _damagedState.Reset_State();
        Reset_CurrentUnitState(_damagedState);
    }

    public void Set_Die()
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _currrentState._nextState = _dieState;
        _dieState.Reset_State();
        Reset_CurrentUnitState(_dieState);
    }

    public void Set_Idle()
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _currrentState._nextState = _idleState;
        _idleState.Reset_State();
        Reset_CurrentUnitState(_idleState);
    }

    public void Set_Move()
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _currrentState._nextState = _moveState;
        _moveState.Reset_State();
        Reset_CurrentUnitState(_moveState);
    }

    public void Set_Throw()
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _currrentState._nextState = _throwState;
        _throwState.Reset_State();
        Reset_CurrentUnitState(_throwState);
    }

    public void Set_Wait(float time)
    {
        _currrentState.Set_Event(eEvent.EXIT);
        _waitState.Set_Time(time);
        _waitState.Set_ExtraTime(_waitExtraTime);
        _currrentState._nextState = _waitState;
        _waitState.Reset_State();
        Reset_CurrentUnitState(_waitState);
    }


    public void Set_WaitExtraTime(float extraTime)
    {
        this._waitExtraTime = extraTime;
    }

    public void Set_ThrowPos(Vector2 pos)
    {
        this._throwState.Set_ThrowPos(pos);
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
