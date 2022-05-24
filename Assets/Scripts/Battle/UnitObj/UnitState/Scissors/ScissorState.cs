using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class ScissorState : AbstractStateManager
	{
		public CardData ScissorPieceData => _scissorPieceData;
		private CardData _scissorPieceData = null;

		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new ScissorIdleState();
			_waitState = new ScissorWaitState();
			_moveState = new ScissorMoveState();
			_attackState = new ScissorAttackState();
			_damagedState = new ScissorDamagedState();
			_dieState = new ScissorDieState();
			_throwState = new ScissorThrowState();

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

	public class ScissorIdleState : AbstractIdleState
	{

	}

	public class ScissorWaitState : AbstractWaitState
	{
	}

	public class ScissorMoveState : AbstractMoveState
	{
	}

	public class ScissorAttackState : AbstractAttackState
	{
		protected override void SetAttackData(ref AtkData atkData)
		{
			base.SetAttackData(ref atkData);
			atkData.Reset_Type(EffAttackType.PCKill);
		}
	}

	public class ScissorDamagedState : AbstractDamagedState
	{
	}

	public class ScissorDieState : WillDieState
	{

	}

	public class ScissorThrowState : AbstractThrowState
	{

	}


}
