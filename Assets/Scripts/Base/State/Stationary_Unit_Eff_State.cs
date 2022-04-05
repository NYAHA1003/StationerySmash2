using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
public abstract class EffState
{
    public eState curState { get; protected set; }
    public eEvent curEvent;
    public Transform myTrm { get; protected set; }
    public Transform mySprTrm { get; protected set; }
    public AtkType statusType { get; protected set; }

    public BattleManager battleManager;
    protected Unit myUnit;
    protected UnitData myUnitData;
    protected UnitStateEff _unitStateEff = null;
    protected IStateManager _stateManager = null;
    protected float[] valueList;
    protected IEffect effectObj;

    public EffState()
    {
    }

    public void SetStateEff(Transform myTrm, Transform mySprTrm, Unit myUnit, AtkType statusEffect, params float[] valueList)
    {
        curEvent = eEvent.ENTER;
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
        this._unitStateEff = myUnit.UnitStateEff;
        _stateManager = myUnit.UnitStateChanger.StateManager;

        Set_EffType(statusEffect, valueList);
        Set_EffValue(valueList);
        this.battleManager = myUnit.BattleManager;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() 
    {
        Delete_StatusEffect();
        curEvent = eEvent.EXIT; 
    }

    public void Process()
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
        }
    }

    public void Set_EffType(AtkType atkType, params float[] value)
    {
        statusType = atkType;
        this.valueList = value;
    }

    public abstract void Set_EffValue(params float[] value);

    /// <summary>
    /// 효과 삭제
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="eff_State"></param>
    public void Delete_StatusEffect()
    {
        if(effectObj != null)
        {
            effectObj.Delete_Effect();
            effectObj = null;
        }

        _unitStateEff.RemoveStateEff(this);

        switch (statusType)
        {
            case AtkType.Normal:
                break;
            case AtkType.Stun:
                PoolManager.AddEff((Sturn_Eff_State)this);
                break;
            case AtkType.Ink:
                PoolManager.AddEff((Ink_Eff_State)this);
                break;
            case AtkType.SlowDown:
                PoolManager.AddEff((SlowDown_Eff_State)this);
                break;
        }

    }
}
