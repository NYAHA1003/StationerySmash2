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

    public UnitState nextState; // 다음 상태

    public Transform myTrm { get; protected set; }
    public Transform mySprTrm { get; protected set; }
    public Unit myUnit { get; protected set; }
    public IStateManager stateChange;
    public BattleManager battleManager;

    public UnitState(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
        this.battleManager = myUnit._battleManager;
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
        this.stateChange = stateChange;
    }

    public void Reset_State()
    {
        nextState = null;
        curEvent = eEvent.ENTER;
    }
    public virtual void Run_Damaged(AtkData atkData)
    {
        if (atkData.damageId.Equals(-1))
        {
            //무조건 무시해야할 공격
            return;
        }
        if (atkData.damageId.Equals(myUnit.myDamagedId))
        {
            //똑같은 공격 아이디를 지닌 공격은 무시함
            return;
        }
        this.stateChange.Set_Damaged(atkData);
    }
    /// <summary>
    /// 효과 추가
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void Add_StatusEffect(AtkType atkType, params float[] value)
    {
        //이미 있는 효과인지 찾기
        Eff_State statEffState = myUnit.statEffList.Find(x => x.statusType.Equals(atkType));
        if (statEffState != null)
        {
            statEffState.Set_EffValue(value);
            return;
        }

        //기존 효과가 없으면 추가
        switch (atkType)
        {
            case AtkType.Normal:
                return;
            case AtkType.Stun:
                myUnit.statEffList.Add(PoolManager.GetEff<Sturn_Eff_State>(myTrm, mySprTrm, myUnit, atkType, value));
                return;
            case AtkType.Ink:
                myUnit.statEffList.Add(PoolManager.GetEff<Ink_Eff_State>(myTrm, mySprTrm, myUnit, atkType, value));
                return;
            case AtkType.SlowDown:
                myUnit.statEffList.Add(PoolManager.GetEff<SlowDown_Eff_State>(myTrm, mySprTrm, myUnit, atkType, value));
                return;
        }
    }
    public virtual Unit Pull_Unit()
    {
        if (myUnit.isDontThrow)
            return null;
        if (curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        stateChange.Set_Wait(2);
        return myUnit;
    }
    public virtual Unit Pulling_Unit()
    {
        if (myUnit.isDontThrow)
            return null;
        if (curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        return myUnit;
    }
    public virtual void Throw_Unit(Vector2 pos)
    {
        stateChange.Set_ThrowPos(pos);
        stateChange.Set_Throw();
    }

}
