using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;
public class Stationary_UnitState : UnitState
{
    public new Stationary_Unit myUnit;
    public UnitData myUnitData { get; protected set; }
    public StageData stageData { get; protected set; }

    public AtkType originAtkType;
    protected float[] originValue;


    public Stationary_UnitState(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit)
    {
        this.myUnit = myUnit;
        this.stageData = stageData;
        this.myUnitData = myUnit.unitData;
    }
    public void Check_Wall()
    {
        if (stageData.max_Range <= myTrm.position.x)
        {
            //¿ÞÂÊÀ¸·Î Æ¨°ÜÁ® ³ª¿È
            myTrm.DOKill();
            myTrm.DOJump(new Vector3(myTrm.position.x - 0.2f, 0, myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
            {
                nextState = stateChange.Return_Wait(this, 0.5f);
            }).SetEase(myUnit.curve);
        }
        if (-stageData.max_Range >= myTrm.position.x)
        {
            //¿À¸¥ÂÊÀ¸·Î Æ¨°ÜÁ® ³ª¿È
            myTrm.DOKill();
            myTrm.DOJump(new Vector3(myTrm.position.x + 0.2f, 0, myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
            {
                nextState = stateChange.Return_Wait(this, 0.5f);
            }).SetEase(myUnit.curve);
        }
    }
}