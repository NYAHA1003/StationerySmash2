using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class RulerState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new RulerStateIdleState();
            _waitState = new RulerStateWaitState();
            _moveState = new RulerStateMoveState();
            _attackState = new RulerStateAttackState();
            _damagedState = new RulerStateDamagedState();
            _dieState = new RulerStateDieState();
            _throwState = new RulerStateThrowState();

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
            myUnit.SetIsDontThrow(true);
            myUnit.SetIsNeverDontThrow(true);
        }
    }

    public class RulerStateIdleState : AbstractIdleState
    {
    }

    public class RulerStateWaitState : AbstractWaitState
    {
    }

    public class RulerStateMoveState : AbstractMoveState
    {
    }

    public class RulerStateAttackState : AbstractAttackState
    {
    }

    public class RulerStateDamagedState : BlockDamagedState
    {
    }

    public class RulerStateDieState : AbstractDieState
    {
    }

    public class RulerStateThrowState : AbstractThrowState
    {

    }
}

