using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class SummonAttackState : AbstractAttackState
{
    /// <summary>
    /// ��ȯ
    /// </summary>
    protected override void Attack()
    {
        //���� �ִϸ��̼�
        Animation();

        //���� ������ �ʱ�ȭ
        _currentdelay = 0;
        SetUnitDelayAndUI();

        //��� ���·� ���ư�
        _stateManager.Set_Wait(0.4f);
        _curEvent = eEvent.EXIT;
    }
}
