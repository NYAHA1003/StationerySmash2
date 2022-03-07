using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class PencilCase_Normal_State : UnitState
{
    public PencilCase_Normal_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {

    }

    public override void Update()
    {
        nextState = new PencilCase_Normal_Idle_State(myTrm, mySprTrm, myUnit);
        curEvent = eEvent.EXIT;
    }
}

public class PencilCase_Normal_Idle_State : PencilCase_Normal_State
{
    public PencilCase_Normal_Idle_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {

    }
}

public class PencilCase_Normal_Damaged_State : PencilCase_Normal_State
{
    private AtkData atkData;
    public PencilCase_Normal_Damaged_State(Transform myTrm, Transform mySprTrm, Unit myUnit, AtkData atkData) : base(myTrm, mySprTrm, myUnit)
    {
        this.atkData = atkData;
    }

    public override void Enter()
    {
        myUnit.Subtract_HP(atkData.damage);

        base.Enter();
    }
    public override void Update()
    {
        nextState = new PencilCase_Normal_Idle_State(myTrm, mySprTrm, myUnit);
        curEvent = eEvent.EXIT;
        if(myUnit.hp <= 0)
        {
            nextState = new PencilCase_Normal_Die_State(myTrm, mySprTrm, myUnit);
            return;
        }
    }
}

public class PencilCase_Normal_Die_State : PencilCase_Normal_State
{
    public PencilCase_Normal_Die_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {

    }
}