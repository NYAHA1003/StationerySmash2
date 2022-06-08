using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class EraserState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new EraserIdleState();
            _waitState = new EraserWaitState();
            _moveState = new EraserMoveState();
            _attackState = new EraserAttackState();
            _damagedState = new EraserDamagedState();
            _dieState = new EraserDieState();
            _throwState = new EraserThrowState();

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

    public class EraserIdleState : AbstractIdleState
    {
    }

    public class EraserWaitState : AbstractWaitState
    {
    }

    public class EraserMoveState : AbstractMoveState
    {
    }

    public class EraserAttackState : PcKillAttackState
    {
    }

    public class EraserDamagedState : AbstractDamagedState
    {
    }

    public class EraserDieState : WillDieState
    {
        protected override void Will()
        {
            base.Will();

            //지우개 조각 소환
            var eraserPiece = DeckDataManagerSO.FindCardData(CardNamingType.EraserPiece);
            _myUnit.BattleManager.UnitComponent.SummonUnit(eraserPiece, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);

        }
    }


    public class EraserThrowState : AbstractThrowState
    {

    }
}

