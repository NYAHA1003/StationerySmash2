using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractWaitState : AbstractUnitState
{
    private float waitTime;
    private float extraWaitTime;

    public void Set_Time(float waitTime)
    {
        this.waitTime = waitTime;
    }
    public void Set_ExtraTime(float extraWaitTime)
    {
        this.extraWaitTime = extraWaitTime;
        this.waitTime = waitTime + extraWaitTime;
    }

    public override void Enter()
    {
        _curState = eState.WAIT;
        _curEvent = eEvent.ENTER;
        _mySprTrm.DOKill();
        base.Enter();
    }

    public override void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        _stateManager.Set_Move();
    }
}
