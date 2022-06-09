using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class SharpLeadCaseState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new SharpLeadCaseIdleState();
            _waitState = new SharpLeadCaseWaitState();
            _moveState = new SharpLeadCaseMoveState();
            _attackState = new SharpLeadCaseAttackState();
            _damagedState = new SharpLeadCaseDamagedState();
            _dieState = new SharpLeadCaseDieState();
            _throwState = new SharpLeadCaseThrowState();

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

    public class SharpLeadCaseIdleState : AbstractIdleState
    {
    }

    public class SharpLeadCaseWaitState : AbstractWaitState
    {
    }

    public class SharpLeadCaseMoveState : AbstractMoveState
    {
    }


    public class SharpLeadCaseAttackState : CritAttackState
    {
        protected override void SetAttackData(ref AtkData atkData)
        {
            base.SetAttackData(ref atkData);
            if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy() / 10)
            {
                //크리티컬
                atkData.Reset_Damage(atkData.damage * 2);
            }
            else
            {
                atkData.Reset_Damage(atkData.damage);
            }
        }
    }

    public class SharpLeadCaseDamagedState : AbstractDamagedState
    {
    }

    public class SharpLeadCaseDieState : AbstractDieState
    {
    }

    public class SharpLeadCaseThrowState : AbstractThrowState
    {

    }
}

