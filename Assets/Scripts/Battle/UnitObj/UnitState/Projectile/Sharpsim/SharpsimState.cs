using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;


namespace Battle.Units
{
	public class SharpsimState : AbstractStateManager
	{
		public override void SetState()
		{
			//������Ʈ���� �����Ѵ�
			_idleState = new SharpsimIdleState();
			_waitState = new SharpsimWaitState();
			_moveState = new SharpsimMoveState();
			_damagedState = new SharpsimDamagedState();

			Reset_CurrentUnitState(_idleState);

			_abstractUnitStateList.Add(_idleState);
			_abstractUnitStateList.Add(_waitState);
			_abstractUnitStateList.Add(_moveState);
			_abstractUnitStateList.Add(_damagedState);

			SetInStateList();
		}

		public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			base.Reset_State(myTrm, mySprTrm, myUnit);
			myUnit.SetIsNeverDontThrow(true);
			myUnit.SetIsInvincibility(true);
		}

		public override void Set_Die()
		{
			//�״°� ����
		}


		public override void Set_Throw()
		{
			//������ ����
		}
		public override void Set_ThrowPos(Vector2 pos)
		{
			//������ ����
		}
	}

	public class SharpsimIdleState : AbstractIdleState
	{
		protected override void IdleToWaitTime()
		{
			//��� ���·� �����
			_stateManager.Set_Wait(0);
		}
	}

	public class SharpsimWaitState : AbstractWaitState
	{
	}

	public class SharpsimMoveState : IgnoreMoveState
	{
		protected override void CheckTargetUnit(Unit targetUnit)
		{
			AtkData atkData = new AtkData(null, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData._dir, _myUnit.ETeam == TeamType.MyTeam, 0, EffAttackType.Normal, EffectType.Attack);
			targetUnit.Run_Damaged(atkData);

			//���� ����
			ResetSprTrm();
			_curEvent = eEvent.EXIT;
			_myUnit.Delete_Unit();
		}
	}

	public class SharpsimDamagedState : AbstractDamagedState
	{
		public override void Enter()
		{
			_stateManager.Set_Wait(0.1f);
		}
	}
}