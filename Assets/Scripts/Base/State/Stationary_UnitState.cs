using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;
public class Stationary_UnitState : UnitState
{
    public UnitData myUnitData { get; protected set; }
    public StageData stageData { get; protected set; }

    public AtkType originAtkType;
    protected float[] originValue;


    public Stationary_UnitState(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        this.myUnit = myUnit;
        this.stageData = stageData;
        this.myUnitData = myUnit.unitData;
        originValue = myUnitData.unitablityData;
        stageData = myUnit._battleManager.CurrentStageData;
    }

    public virtual void Animation(params float[] value)
    {

    }

    public void Check_Wall()
    {
        if (stageData.max_Range <= myTrm.position.x)
        {
            //�������� ƨ���� ����
            myTrm.DOKill();
            myTrm.DOJump(new Vector3(myTrm.position.x - 0.2f, 0, myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
            {
                stateChange.Set_Wait(0.5f);
            }).SetEase(Utill.Parabola.Return_ParabolaCurve());
        }
        if (-stageData.max_Range >= myTrm.position.x)
        {
            //���������� ƨ���� ����
            myTrm.DOKill();
            myTrm.DOJump(new Vector3(myTrm.position.x + 0.2f, 0, myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
            {
                stateChange.Set_Wait(0.5f);
            }).SetEase(Utill.Parabola.Return_ParabolaCurve());
        }
    }
}