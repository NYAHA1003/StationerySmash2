using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;


namespace Battle.Units
{

	public class FlyKillAttackState : AbstractAttackState
	{
		/// <summary>
		/// 타겟과의 거리 체크
		/// </summary>
		protected override void CheckRangeToTarget()
		{
			if (_targetUnit == null)
			{
				return;
			}
			if (Vector2.Distance(_myTrm.position, _targetUnit.transform.position) <= _myUnit.UnitStat.Return_Range())
			{
				return;
			}

			if (_myUnit.ETeam == TeamType.MyTeam && _myTrm.position.x <= _targetUnit.transform.position.x)
			{
				return;
			}
			if (_myUnit.ETeam == TeamType.EnemyTeam && _myTrm.position.x >= _targetUnit.transform.position.x)
			{
				return;
			}
			_stateManager.Set_Move();
		}
	}

}