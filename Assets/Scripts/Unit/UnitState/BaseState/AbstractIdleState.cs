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

        //��ȯ�� �ִϸ��̼�
        Animation();

        //��� ���·� �����
        _stateManager.Set_Wait(0.5f);
    }
}
