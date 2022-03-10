using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public abstract class UnitState
{
    public eState curState { get; protected set; }
    public eEvent curEvent;

    public UnitState nextState; // 다음 상태

    public Transform myTrm { get; protected set; }
    public Transform mySprTrm { get; protected set; }
    public Unit myUnit { get; protected set; }
    public IStateChange stateChange;

    public UnitState(Transform myTrm, Transform mySprTrm, Unit myUnit)
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

    public void Set_StateChange(IStateChange stateChange)
    {
        this.stateChange = stateChange;
    }

    public void Reset_State()
    {
        nextState = null;
        curEvent = eEvent.ENTER;
    }
}
