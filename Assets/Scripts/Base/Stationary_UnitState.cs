using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Stationary_UnitState : UnitState
{
    public new Stationary_Unit myUnit { get; protected set; }
    public new Stationary_UnitState nextState;
    public UnitData myUnitData { get; protected set; }
    public StageData stageData { get; protected set; }


    public AtkType originAtkType;
    protected float[] originValue;


    public Stationary_UnitState(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit)
    {
        this.stageData = stageData;
        this.myUnitData = myUnit.unitData;
    }

    new public Stationary_UnitState Process()
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
}