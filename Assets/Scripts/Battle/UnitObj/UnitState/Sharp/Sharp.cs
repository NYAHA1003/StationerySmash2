using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class SharpState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new SharpIdleState();
            _waitState = new SharpWaitState();
            _moveState = new SharpMoveState();
            _attackState = new SharpAttackState();
            _damagedState = new SharpDamagedState();
            _dieState = new SharpDieState();
            _throwState = new SharpThrowState();

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

    public class SharpIdleState : AbstractIdleState
    {
    }

    public class SharpWaitState : AbstractWaitState
    {
    }

    public class SharpMoveState : AbstractMoveState
    {
    }

    public class SharpAttackState : SummonAttackState
    {
        protected override void Summon()
        {
            //샤프심 소환
            var sharpsimData = DeckDataManagerSO.FindStdCardData(CardNamingType.SharpSim);
            _myUnit.BattleManager.UnitComponent.SummonUnit(sharpsimData, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);
        }
    }

    public class SharpDamagedState : AbstractDamagedState
    {
    }

    public class SharpDieState : AbstractDieState
    {
    }

    public class SharpThrowState : AbstractThrowState
    {

    }
}

