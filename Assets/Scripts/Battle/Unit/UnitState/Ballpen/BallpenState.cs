using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class BallpenState : AbstractStateManager
	{
		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new BallPenIdleState();
			_waitState = new BallPenWaitState();
			_moveState = new BallPenMoveState();
			_attackState = new BallPenAttackState();
			_damagedState = new BallPenDamagedState();
			_dieState = new BallPenDieState();
			_throwState = new BallPenThrowState();

			Reset_CurrentUnitState(_idleState);

			_idleState.SetStateManager(this);
			_waitState.SetStateManager(this);
			_moveState.SetStateManager(this);
			_attackState.SetStateManager(this);
			_damagedState.SetStateManager(this);
			_dieState.SetStateManager(this);
			_throwState.SetStateManager(this);

			_idleState.SetAnimation();
			_waitState.SetAnimation();
			_moveState.SetAnimation();
			_attackState.SetAnimation();
			_damagedState.SetAnimation();
			_dieState.SetAnimation();
			_throwState.SetAnimation();
		}

		public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			base.Reset_State(myTrm, mySprTrm, myUnit);
			myUnit.SetIsNeverDontThrow(false);
		}
	}

	public class BallPenIdleState : AbstractIdleState
	{
	}

	public class BallPenWaitState : AbstractWaitState
	{
	}

	public class BallPenMoveState : AbstractMoveState
	{
	}

	public class BallPenAttackState : AbstractAttackState
	{
	}

	public class BallPenDamagedState : AbstractDamagedState
	{
	}

	public class BallPenDieState : AbstractDieState
	{
	}

	public class BallPenThrowState : AbstractThrowState
	{
		private float _inkRange = 0.5f;
		protected override void ThrowAttack(Unit targetUnit)
		{
			base.ThrowAttack(targetUnit);
			switch (_myUnit.ETeam)
			{
				case TeamType.Null:
					break;
				case TeamType.MyTeam:
					InkFlooding(_myUnit.BattleManager.CommandUnit._enemyUnitList);
					break;
				case TeamType.EnemyTeam:
					InkFlooding(_myUnit.BattleManager.CommandUnit._playerUnitList);
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
					targetUnit.AddStatusEffect(EffAttackType.Ink, 1, 20, 20);
				}
			}
		}
	}

}