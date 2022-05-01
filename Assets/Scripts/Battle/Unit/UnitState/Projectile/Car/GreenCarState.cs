using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;


namespace Battle.Units
{

	public class GreenCarState : AbstractStateManager
	{
		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new GreenCarIdleState();
			_waitState = new GreenCarWaitState();
			_moveState = new GreenCarMoveState();
			_damagedState = new GreenCarDamagedState();

			Reset_CurrentUnitState(_idleState);

			_abstractUnitStateList.Add(_idleState);
			_abstractUnitStateList.Add(_waitState);
			_abstractUnitStateList.Add(_moveState);
			_abstractUnitStateList.Add(_damagedState);

			SetInStateList();
		}
		public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			myUnit.SetIsNeverDontThrow(true);
			_idleState.ChangeUnit(myTrm, mySprTrm, myUnit);
			_waitState.ChangeUnit(myTrm, mySprTrm, myUnit);
			_moveState.ChangeUnit(myTrm, mySprTrm, myUnit);
			_damagedState.ChangeUnit(myTrm, mySprTrm, myUnit);

			_idleState.ResetState();
			_waitState.ResetState();
			_moveState.ResetState();
			_damagedState.ResetState();

			Set_WaitExtraTime(0);
			Reset_CurrentUnitState(_idleState);
		}

		public override void Set_Die()
		{
			//죽는거 없음
		}


		public override void Set_Throw()
		{
			//던지기 무시
		}
		public override void Set_ThrowPos(Vector2 pos)
		{
			//던지기 무시
		}
	}

	public class GreenCarIdleState : AbstractIdleState
	{
	}

	public class GreenCarWaitState : AbstractWaitState
	{
	}

	public class GreenCarMoveState : IgnoreMoveState
	{
		protected override void CheckTargetUnit(Unit targetUnit)
		{
			AtkData atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, EffAttackType.Normal, EffectType.Attack, originValue);
			targetUnit.Run_Damaged(atkData);
		}
	}

	public class GreenCarDamagedState : AbstractDamagedState
	{
		public override void Enter()
		{
			_stateManager.Set_Wait(0.1f);
		}
	}
}