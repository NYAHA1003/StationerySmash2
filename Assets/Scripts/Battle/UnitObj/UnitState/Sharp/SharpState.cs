using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class SharpState : AbstractStateManager
	{
		public override void SetState()
		{
			//������Ʈ���� �����Ѵ�
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
			//������ ��ȯ
			var sharpsimData = DeckDataManagerSO.FindCardData(CardNamingType.SharpSim);
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