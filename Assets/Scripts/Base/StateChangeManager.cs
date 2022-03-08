using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class StateChangeManager
{
    static public Stationary_UnitState Set_Idle(Stationary_UnitState unit)
    {
        unit.curEvent = UnitState.eEvent.EXIT;

        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Idle_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);

            case UnitType.BallPen:
                return new BallPen_Idle_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        }
    }

    static public Stationary_UnitState Set_Wait(Stationary_UnitState unit, float waitTime)
    {
        unit.curEvent = UnitState.eEvent.EXIT;

        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Wait_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, waitTime);

            case UnitType.BallPen:
                return new BallPen_Wait_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, waitTime);
        }
    }

    static public Stationary_UnitState Set_Move(Stationary_UnitState unit)
    {
        unit.curEvent = UnitState.eEvent.EXIT;

        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Move_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);

            case UnitType.BallPen:
                return new BallPen_Move_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        }
    }

    static public Stationary_UnitState Set_Damaged(Stationary_UnitState unit, AtkData atkData)
    {
        unit.curEvent = UnitState.eEvent.EXIT;

        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Damaged_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, atkData);

            case UnitType.BallPen:
                return new BallPen_Damaged_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, atkData);
        }
    }

    static public Stationary_UnitState Set_Attack(Stationary_UnitState unit, Unit targetUnit)
    {
        unit.curEvent = UnitState.eEvent.EXIT;
        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Attack_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, targetUnit);

            case UnitType.BallPen:
                return new BallPen_Attack_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, targetUnit);
        }
    }

    static public Stationary_UnitState Set_Die(Stationary_UnitState unit)
    {
        unit.curEvent = UnitState.eEvent.EXIT;
        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Die_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);

            case UnitType.BallPen:
                return new BallPen_Die_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        }
    }

    static public Stationary_UnitState Set_Throw(Stationary_UnitState unit)
    {
        unit.curEvent = UnitState.eEvent.EXIT;
        switch (unit.myUnitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                return new Pencil_Throw_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);

            case UnitType.BallPen:
                return new BallPen_Throw_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
        }
    }
}