using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public abstract class WillDieState : AbstractDieState
{
    public override void Enter()
    {
        //딜레이바 등의 UI 안 보이게 하고 상태이상 삭제
        _myUnit.UnitStateEff.DeleteEffStetes();
        _myUnit.SetIsDontThrow(true);
        _myUnit.UnitSprite.ShowCanvas(false);

        //뒤짐
        _myUnit.SetIsInvincibility(true);
        _myTrm.DOKill();
        _mySprTrm.DOKill();

        //유언 효과 발동
        Will();

        _curEvent = eEvent.UPDATE;
    }

    /// <summary>
    /// 유언 효과 발동
    /// </summary>
    protected virtual void Will()
    {
        //랜덤으로 죽는 애니메이션 재생
        RandomDieAnimation();
    }
}
