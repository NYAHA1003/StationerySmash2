using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class UnitState
{
    public enum eState  // 가질 수 있는 상태 나열
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE, PULL, THROW, NONE,
    };

    public enum eEvent  // 이벤트 나열
    {
        ENTER, UPDATE, EXIT, NOTHING
    };

    public eState curState { get; protected set; }
    public eEvent curEvent;

    public UnitState nextState; // 다음 상태

    public Transform myTrm { get; protected set; }
    public Transform mySprTrm { get; protected set; }
    public Unit myUnit { get; protected set; }

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
    public UnitState Process()
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

    public void Set_Event(eEvent eEvent)
    {
        curEvent = eEvent;
    }
}
