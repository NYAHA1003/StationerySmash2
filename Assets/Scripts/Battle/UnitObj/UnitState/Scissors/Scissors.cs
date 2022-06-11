using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class ScissorsState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new ScissorsIdleState();
            _waitState = new ScissorsWaitState();
            _moveState = new ScissorsMoveState();
            _attackState = new ScissorsAttackState();
            _damagedState = new ScissorsDamagedState();
            _dieState = new ScissorsDieState();
            _throwState = new ScissorsThrowState();

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

    public class ScissorsIdleState : AbstractIdleState
    {
    }

    public class ScissorsWaitState : AbstractWaitState
    {
    }

    public class ScissorsMoveState : AbstractMoveState
    {
    }

    public class ScissorsAttackState : PcKillAttackState
    {
    }

    public class ScissorsDamagedState : AbstractDamagedState
    {
    }

    public class ScissorsDieState : AbstractDieState
    {
    }

    public class ScissorsThrowState : AbstractThrowState
    {

    }
}

