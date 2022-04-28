using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class EraserState : AbstractStateManager
	{
		public CardData EraserPieceData => _eraserPieceData;
		private CardData _eraserPieceData = null;

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
			_eraserPieceData = AddressableTool.ReturnProjectileUnit(UnitType.EraserPiece).Result;
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

	public class EraserAttackState : AbstractAttackState
	{
	}

	public class EraserDamagedState : AbstractDamagedState
	{
	}

	public class EraserDieState : WillDieState
	{
		protected override void Will()
		{
			//지우개 조각 소환
			EraserState eraserState = (EraserState)_stateManager;
			_myUnit.BattleManager.CommandUnit.SummonUnit(eraserState.EraserPieceData, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);

			base.Will();
		}
	}

	public class EraserThrowState : AbstractThrowState
	{

	}


}