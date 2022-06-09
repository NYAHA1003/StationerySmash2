using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class CutterKnifeState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new CutterKnifeIdleState();
            _waitState = new CutterKnifeWaitState();
            _moveState = new CutterKnifeMoveState();
            _attackState = new CutterKnifeAttackState();
            _damagedState = new CutterKnifeDamagedState();
            _dieState = new CutterKnifeDieState();
            _throwState = new CutterKnifeThrowState();

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

    public class CutterKnifeIdleState : AbstractIdleState
    {
    }

    public class CutterKnifeWaitState : AbstractWaitState
    {
    }

    public class CutterKnifeMoveState : AbstractMoveState
    {
    }

    public class CutterKnifeAttackState : SeFiceAttackState
    {
        protected override void SetAttackData(ref AtkData atkData)
        {
            base.SetAttackData(ref atkData);
            _myUnit.SubtractHP(_seFiceDamaged);

        }
    }

    public class CutterKnifeDamagedState : AbstractDamagedState
    {
    }

    public class CutterKnifeDieState : AbstractDieState
    {
    }

    public class CutterKnifeThrowState : AbstractThrowState
    {

    }
}

