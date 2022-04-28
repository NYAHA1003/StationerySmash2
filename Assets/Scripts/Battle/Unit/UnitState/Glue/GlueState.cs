using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{


	public class GlueState : AbstractStateManager
	{

		public override void SetState()
		{
			//스테이트들을 설정한다
			_idleState = new GlueIdleState();
			_waitState = new GlueWaitState();
			_moveState = new GlueMoveState();
			_attackState = new GlueAttackState();
			_damagedState = new GlueDamagedState();
			_dieState = new GlueDieState();
			_throwState = new GlueThrowState();

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


	public class GlueIdleState : AbstractIdleState
	{
	}

	public class GlueWaitState : AbstractWaitState
	{
	}

	public class GlueMoveState : AbstractMoveState
	{
	}

	public class GlueAttackState : AbstractAttackState
	{
		protected override void SetAttackData(ref AtkData atkData)
		{
			atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, EffAttackType.SlowDown, _myUnit.SkinData._effectType, originValue);
		}
	}

	public class GlueDamagedState : AbstractDamagedState
	{
	}

	public class GlueDieState : AbstractDieState
	{
	}

	public class GlueThrowState : AbstractThrowState
	{

	}

}
