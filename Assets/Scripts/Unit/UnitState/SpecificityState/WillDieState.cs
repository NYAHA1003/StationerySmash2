using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public abstract class WillDieState : AbstractDieState
{
    public override void Enter()
    {
        //�����̹� ���� UI �� ���̰� �ϰ� �����̻� ����
        _myUnit.UnitStateEff.DeleteEffStetes();
        _myUnit.SetIsDontThrow(true);
        _myUnit.UnitSprite.ShowCanvas(false);

        //����
        _myUnit.SetIsInvincibility(true);
        _myTrm.DOKill();
        _mySprTrm.DOKill();

        //���� ȿ�� �ߵ�
        Will();

        _curEvent = eEvent.UPDATE;
    }

    /// <summary>
    /// ���� ȿ�� �ߵ�
    /// </summary>
    protected virtual void Will()
    {
        //�������� �״� �ִϸ��̼� ���
        RandomDieAnimation();
    }
}
