using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;
public abstract class AbstractIdleState : AbstractUnitState
{
    public override void Enter()
    {
        _curState = eState.IDLE;
        _curEvent = eEvent.ENTER;

        //소환시 애니메이션
        Animation();

        //대기 상태로 만든다
        _stateManager.Set_Wait(0.5f);
    }
}
