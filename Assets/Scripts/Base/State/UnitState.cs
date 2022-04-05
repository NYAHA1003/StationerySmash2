using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;
using Battle;

public abstract class UnitState
{
    public eState curState { get; protected set; }
    public eEvent curEvent;

    public UnitState nextState = null; // 다음 상태

    public Transform myTrm { get; protected set; } = null;
    public Transform mySprTrm { get; protected set; } = null;
    public Unit myUnit { get; protected set; } = null;
    public IStateManager _stateManager = null;

    public UnitState(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
    }

    public void Change_Trm(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    /// <summary>
    /// 로직 실행
    /// </summary>
    /// <returns></returns>
    public virtual UnitState Process()
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

    public void Set_Event(eEvent eEvent)
    {
        curEvent = eEvent;
    }

    public void Set_StateChange(IStateManager stateChange)
    {
        this._stateManager = stateChange;
    }

    public void Reset_State()
    {
        nextState = null;
        curEvent = eEvent.ENTER;
    }
    public virtual void Run_Damaged(AtkData atkData)
    {
        if (atkData.damageId == -1)
        {
            //무조건 무시해야할 공격
            return;
        }
        if (atkData.damageId == myUnit.MyDamagedId)
        {
            //똑같은 공격 아이디를 지닌 공격은 무시함
            return;
        }
        this._stateManager.Set_Damaged(atkData);
    }
    public virtual Unit Pull_Unit()
    {
        if (myUnit._isDontThrow)
            return null;
        if (curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        _stateManager.Set_Wait(2);
        return myUnit;
    }
    public virtual Unit Pulling_Unit()
    {
        if (myUnit._isDontThrow)
            return null;
        if (curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        return myUnit;
    }
    public virtual void Throw_Unit(Vector2 pos)
    {
        _stateManager.Set_ThrowPos(pos);
        _stateManager.Set_Throw();
    }

}
