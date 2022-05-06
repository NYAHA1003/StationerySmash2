using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;


namespace Battle.Units
{

	public class RullerState : AbstractStateManager
	{
		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new RullerIdleState();
			_waitState = new RullerWaitState();
			_moveState = new RullerMoveState();
			_attackState = new RullerAttackState();
			_damagedState = new RullerDamagedState();
			_dieState = new RullerDieState();
			_throwState = new RullerThrowState();

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

	public class RullerIdleState : AbstractIdleState
	{
	}

	public class RullerWaitState : AbstractWaitState
	{
	}

	public class RullerMoveState : AbstractMoveState
	{
	}

	public class RullerAttackState : SeFiceAttackState
	{
		
	}

	public class RullerDamagedState : AbstractDamagedState
	{
	}

	public class RullerDieState : AbstractDieState
	{
	}

	public class RullerThrowState : AbstractThrowState
	{

	}

}
