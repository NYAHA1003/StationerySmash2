using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class PostItPieceState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new PostItPieceIdleState();
            _waitState = new PostItPieceWaitState();
            _moveState = new PostItPieceMoveState();
            _attackState = new PostItPieceAttackState();
            _damagedState = new PostItPieceDamagedState();
            _dieState = new PostItPieceDieState();
            _throwState = new PostItPieceThrowState();

            Reset_CurrentUnitState(_idleState);

            _abstractUnitStateList.Add(_idleState);
            _abstractUnitStateList.Add(_waitState);
            _abstractUnitStateList.Add(_moveState);
            _abstractUnitStateList.Add(_attackState);
            _abstractUnitStateList.Add(_damagedState);
            _abstractUnitStateList.Add(_dieState);
            _abstractUnitStateList.Add(_throwState);

            SetInStateList();
        }

        public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
        {
            base.Reset_State(myTrm, mySprTrm, myUnit);
            myUnit.SetIsInvincibility(false);
            myUnit.SetIsDontThrow(false);
            myUnit.SetIsNeverDontThrow(false);
        }
    }

    public class PostItPieceIdleState : AbstractIdleState
    {
    }

    public class PostItPieceWaitState : AbstractWaitState
    {
    }

    public class PostItPieceMoveState : AbstractMoveState
    {
    }

    public class PostItPieceAttackState : PcKillAttackState
    {
    }

    public class PostItPieceDamagedState : AbstractDamagedState
    {
    }

    public class PostItPieceDieState : AbstractDieState
    {
    }

    public class PostItPieceThrowState : AbstractThrowState
    {

    }
}

