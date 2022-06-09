using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class PostItState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new PostItIdleState();
            _waitState = new PostItWaitState();
            _moveState = new PostItMoveState();
            _attackState = new PostItAttackState();
            _damagedState = new PostItDamagedState();
            _dieState = new PostItDieState();
            _throwState = new PostItThrowState();

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

    public class PostItIdleState : AbstractIdleState
    {
    }

    public class PostItWaitState : AbstractWaitState
    {
    }

    public class PostItMoveState : AbstractMoveState
    {
    }

    public class PostItAttackState : SummonAttackState
    {
        protected override void Summon()
        {
            //포스트잇 쪼가리
            var postItPiece = DeckDataManagerSO.FindCardData(CardNamingType.PostItPiece);
            _myUnit.BattleManager.UnitComponent.SummonUnit(postItPiece, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);
        }
    }

    public class PostItDamagedState : AbstractDamagedState
    {
    }

    public class PostItDieState : AbstractDieState
    {
    }

    public class PostItThrowState : AbstractThrowState
    {

    }
}

