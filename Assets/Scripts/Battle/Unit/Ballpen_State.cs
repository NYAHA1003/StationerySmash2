using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class BallPen_Idle_State : Pencil_Idle_State
{
    public BallPen_Idle_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {

    }
}

public class BallPen_Wait_State : Pencil_Wait_State
{
    public BallPen_Wait_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData, float waitTime) : base(myTrm, mySprTrm, myUnit, stageData, waitTime)
    {
    }
}

public class BallPen_Move_State : Pencil_Move_State
{
    public BallPen_Move_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Damaged_State : Pencil_Damaged_State
{
    public BallPen_Damaged_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData, AtkData atkData) : base(myTrm, mySprTrm, myUnit, stageData, atkData)
    {
    }
}

public class BallPen_Attack_State : Pencil_Attack_State
{
    public BallPen_Attack_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData, Unit targetUnit) : base(myTrm, mySprTrm, myUnit, stageData, targetUnit)
    {
    }
}

public class BallPen_Die_State : Pencil_Die_State
{
    public BallPen_Die_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }
}

public class BallPen_Throw_State : Pencil_Throw_State
{
    public BallPen_Throw_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        this.originAtkType = myUnit.unitData.atkType;
        this.originValue = myUnit.unitData.unitablityData;
    }

    protected override void Run_ThrowAttack(Unit targetUnit)
    {
        float dir = Vector2.Angle((Vector2)myTrm.position, (Vector2)targetUnit.transform.position);
        float extraKnockBack = (targetUnit.weight - myUnit.Return_Weight() * (float)targetUnit.hp / targetUnit.maxhp) * 0.025f;
        AtkData atkData = new AtkData(myUnit, 0, 0, 0, 0, true, 0, originAtkType, originValue);

        targetUnit.Add_StatusEffect(originAtkType, originValue);
        atkData.Reset_Damage(100 + (myUnit.weight > targetUnit.weight ? (Mathf.RoundToInt((float)myUnit.weight - targetUnit.weight) / 2) : Mathf.RoundToInt((float)(targetUnit.weight - myUnit.weight) / 5)));


        //무게가 더 클 경우
        if (myUnit.weight > targetUnit.weight)
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 더 작을 경우
        if (myUnit.weight < targetUnit.weight)
        {
            atkData.Reset_Kncockback(0, 0, 0, false);
            atkData.Reset_Type(AtkType.Normal);
            atkData.Reset_Value(0);
            targetUnit.Run_Damaged(atkData);

            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 같을 경우
        if (myUnit.weight == targetUnit.weight)
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);


            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);

            return;
        }
    }
}