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
		public CardData SharpsimPieceData => _sharpsimPieceData;
		private CardData _sharpsimPieceData = null;
		private UnitDataSO _unitDataSO = null;
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
			SharpState eraserState = (SharpState)_stateManager;
			_myUnit.BattleManager.CommandUnit.SummonUnit(eraserState.SharpsimPieceData, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);

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