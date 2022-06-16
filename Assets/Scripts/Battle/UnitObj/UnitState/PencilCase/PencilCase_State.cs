using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{

	public class PencilCaseStateManager : AbstractStateManager
	{
		public override void SetState()
		{
			_idleState = new PencilCaseIdleState();
			_damagedState = new PencilCaseDamagedState();
			_dieState = new PencilCaseDieState();

			Reset_CurrentUnitState(_idleState);

			_idleState.SetStateManager(this);
			_damagedState.SetStateManager(this);
			_dieState.SetStateManager(this);
		}

		public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			_animator = myUnit.Animator;
			_idleState.ChangeUnit(myTrm, mySprTrm, myUnit);
			_damagedState.ChangeUnit(myTrm, mySprTrm, myUnit);
			_dieState.ChangeUnit(myTrm, mySprTrm, myUnit);

			_idleState.ResetState();
			_damagedState.ResetState();
			_dieState.ResetState();

			Set_WaitExtraTime(0);
			Reset_CurrentUnitState(_idleState);
		}
	}
	public class PencilCaseIdleState : AbstractIdleState
	{
		public override void Enter()
		{
			_curState = eState.IDLE;
			_curEvent = eEvent.ENTER;
			Animation(eState.IDLE);
		}

		public override Unit PullUnit()
		{
			return null;
		}

		public override Unit PullingUnit()
		{
			return null;
		}
		public override void ThrowUnit(Vector2 pos)
		{

		}
	}
	public class PencilCaseDamagedState : AbstractDamagedState
	{
		public override void Enter()
		{
			_curState = eState.DAMAGED;
			_curEvent = eEvent.ENTER;

			//고유효과 속성이면 효과 적용
			if (_atkData.atkType > EffAttackType.Inherence)
			{
				_myUnit.AddInherence(_atkData);
			}

			//필통 유닛 자료형의 필통 컴포넌트의 피격 이펙트 출력 
			_myUnit.BattleManager.PencilCaseComponent.PlayBloodEffect(_myUnit.ETeam);
			_myUnit.BattleManager.EffectComponent.SetEffect(_atkData._effectType, new EffData(_myTrm.transform.position, 0.2f));
			_myUnit.SubtractHP(_atkData.damage);
			if (_myUnit.UnitStat.Hp <= 0)
			{
				_stateManager.Set_Die();
				Animation(eState.DIE);
				return;
			}
			Animation(eState.DAMAGED);
			_curEvent = eEvent.UPDATE;
		}

		public override void Update()
		{
			_stateManager.Set_Idle();
		}
		public override Unit PullUnit()
		{
			return null;
		}

		public override Unit PullingUnit()
		{
			return null;
		}
		public override void ThrowUnit(Vector2 pos)
		{

		}
	}
	public class PencilCaseDieState : AbstractDieState
	{
		public override void Enter()
		{
			//우리 팀일 경우
			if (_myUnit.ETeam == TeamType.MyTeam)
			{
				_myUnit.BattleManager.WinLoseComponent.SendEndGame(false);
			}
			else if (_myUnit.ETeam == TeamType.EnemyTeam)
			{
				_myUnit.BattleManager.WinLoseComponent.SendEndGame(true);
			}
		}
		public override Unit PullUnit()
		{
			return null;
		}

		public override Unit PullingUnit()
		{
			return null;
		}
		public override void ThrowUnit(Vector2 pos)
		{

		}
	}


}