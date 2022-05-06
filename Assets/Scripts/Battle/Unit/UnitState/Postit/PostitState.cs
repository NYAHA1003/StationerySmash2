using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Units
{

	public class PostitState : AbstractStateManager
	{
		public CardData PostitData => _postitData;
		private CardData _postitData = null;


		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new PostitIdleState();
			_waitState = new PostitWaitState();
			_moveState = new PostitMoveState();
			_attackState = new PostitAttackState();
			_damagedState = new PostitDamagedState();
			_dieState = new PostitDieState();
			_throwState = new PostitThrowState();

			_abstractUnitStateList.Add(_idleState);
			_abstractUnitStateList.Add(_waitState);
			_abstractUnitStateList.Add(_moveState);
			_abstractUnitStateList.Add(_attackState);
			_abstractUnitStateList.Add(_damagedState);
			_abstractUnitStateList.Add(_dieState);
			_abstractUnitStateList.Add(_throwState);

			Reset_CurrentUnitState(_idleState);
			SetInStateList();
		}

		public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			base.Reset_State(myTrm, mySprTrm, myUnit);
			myUnit.SetIsNeverDontThrow(false);
		}
	}


	public class PostitIdleState : AbstractIdleState
	{
		float timer = 0;
		protected override void IdleToWaitTime()
		{
			timer += Time.deltaTime;
			if(timer > 4)
            {
				timer = 0;
				PostitState eraserState = (PostitState)_stateManager;
				_myUnit.BattleManager.CommandUnit.SummonUnit(eraserState.PostitData, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);

				base.IdleToWaitTime();
			}
			//지우개 조각 소환

		}
	}

	public class PostitWaitState : AbstractWaitState
	{
	}

	public class PostitMoveState : AbstractMoveState
	{
	}

	public class PostitAttackState : SummonAttackState
	{
	}

	public class PostitDamagedState : AbstractDamagedState
	{
	}

	public class PostitDieState : AbstractDieState
	{
	}

	public class PostitThrowState : AbstractThrowState
	{

	}

	

}