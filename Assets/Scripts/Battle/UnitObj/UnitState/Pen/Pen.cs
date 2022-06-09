using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{
    public class PenState : AbstractStateManager
    {
        public override void SetState()
        {
            //스테이트들을 설정한다
            _idleState = new PenIdleState();
            _waitState = new PenWaitState();
            _moveState = new PenMoveState();
            _attackState = new PenAttackState();
            _damagedState = new PenDamagedState();
            _dieState = new PenDieState();
            _throwState = new PenThrowState();

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

    public class PenIdleState : AbstractIdleState
    {
    }

    public class PenWaitState : AbstractWaitState
    {
    }

    public class PenMoveState : AbstractMoveState
    {
    }

    public class PenAttackState : PcKillAttackState
    {
    }

    public class PenDamagedState : AbstractDamagedState
    {
    }

    public class PenDieState : AbstractDieState
    {
    }

    public class PenThrowState : AbstractThrowState
    {
        private float _inkRange = 1f;
        protected override void ThrowAttack(Unit targetUnit)
        {
            base.ThrowAttack(targetUnit);
            switch (_myUnit.ETeam)
            {
                case TeamType.Null:
                    break;
                case TeamType.MyTeam:
                    InkFlooding(_myUnit.BattleManager.UnitComponent._enemyUnitList);
                    break;
                case TeamType.EnemyTeam:
                    InkFlooding(_myUnit.BattleManager.UnitComponent._playerUnitList);
                    break;
            }
        }

        private void InkFlooding(List<Unit> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Unit targetUnit = list[i];
                if (Vector2.Distance(_myTrm.position, targetUnit.transform.position) <= _inkRange)
                {
                    targetUnit.AddStatusEffect(EffAttackType.Ink, 6, 10, 20);
                }
            }
        }
    }
}

