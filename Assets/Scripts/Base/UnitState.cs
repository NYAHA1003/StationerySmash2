using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class UnitState
{
    public enum eState  // ���� �� �ִ� ���� ����
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE, PULL, THROW, NONE,
    };

    public enum eEvent  // �̺�Ʈ ����
    {
        ENTER, UPDATE, EXIT, NOTHING
    };

    public eState curState { get; protected set; }
    public AtkType curStatEff { get; protected set; }
    public eEvent curEvent { get; protected set; }

    public UnitState nextState { get; protected set; }  // ���� ����

    protected Transform myTrm;
    protected Transform mySprTrm;
    protected Unit myUnit;

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
    /// ���� ����
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

    public virtual void Set_Idle()
    {
        throw new System.Exception("Set_Idle �Լ��� �������̵����� ����");
    }

    public virtual void Set_Wait(float waitTime)
    {
        throw new System.Exception("Set_Wait �Լ��� �������̵����� ����");
    }

    public virtual void Set_Move()
    {
        throw new System.Exception("Set_Move �Լ��� �������̵����� ����");
    }

    public virtual void Set_Damaged(AtkData atkData)
    {
        throw new System.Exception("Set_Damaged �Լ��� �������̵����� ����");
    }
    public virtual void Set_Die()
    {
        throw new System.Exception("Set_Die �Լ��� �������̵����� ����");
    }
    public virtual void Set_Attack()
    {
        throw new System.Exception("Set_Attack �Լ��� �������̵����� ����");
    }
    public virtual void Set_Pull()
    {
        throw new System.Exception("Set_Pull �Լ��� �������̵����� ����");
    }
    public virtual void Set_Throw()
    {
        throw new System.Exception("Set_Throw �Լ��� �������̵����� ����");
    }
}
