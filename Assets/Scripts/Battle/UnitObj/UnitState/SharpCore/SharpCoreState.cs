using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class SharpCoreState : AbstractStateManager
	{
		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new SharpCoreIdleState();
			_waitState = new SharpCoreWaitState();
			_moveState = new SharpCoreMoveState();
			_attackState = new SharpCoreAttackState();
			_damagedState = new SharpCoreDamagedState();
			_dieState = new SharpCoreDieState();
			_throwState = new SharpCoreThrowState();

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

	public class SharpCoreIdleState : AbstractIdleState
	{
	}

	public class SharpCoreWaitState : AbstractWaitState
	{
	}

	public class SharpCoreMoveState : AbstractMoveState
	{
	}

	public class SharpCoreAttackState : CritAttackState
	{
		protected override void SetAttackData(ref AtkData atkData)
		{
			base.SetAttackData(ref atkData);
			if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy() / 10)
			{
				//크리티컬
				atkData.Reset_Damage(atkData.damage * 2);
			}
			else
			{
				atkData.Reset_Damage(atkData.damage);
			}
		}
	}

	public class SharpCoreDamagedState : AbstractDamagedState
	{
	}

	public class SharpCoreDieState : AbstractDieState
	{
	}

	public class SharpCoreThrowState : AbstractThrowState
	{

	}
}