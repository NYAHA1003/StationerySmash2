using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Stationary_UnitState : UnitState
{
    protected new Stationary_Unit myUnit;
    protected UnitData myUnitData;
    public Stationary_UnitState(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        this.myUnitData = myUnit.unitData;
    }

}