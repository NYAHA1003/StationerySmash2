using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class FlyMoveState : AbstractMoveState
{
    public override void Enter()
    {
        _myUnit.SetIsDontThrow(false);
        _curState = eState.MOVE;
        _curEvent = eEvent.ENTER;

        //이동 애니메이션 시작
        Animation();

        _curEvent = eEvent.UPDATE;
    }

    public override void Animation(params float[] value)
    {
        ResetAnimation();
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? 30 : -30;
        _mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.3f).SetLoops(-1, LoopType.Yoyo);
        _myTrm.DOLocalMoveY(1, 0.5f);
    }

    protected override void CheckTargetUnit(Unit targetUnit)
    {
        _myTrm.DOLocalMoveY(0, 0.3f).OnComplete(() => 
        {
            base.CheckTargetUnit(targetUnit);
        });
    }
}
