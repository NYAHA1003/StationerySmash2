using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;


namespace Battle.Units
{

	public class FlyKillMoveState : AbstractMoveState
	{
		protected override void CheckRange(List<Unit> list)
		{
			float targetRange = float.MaxValue;
			Unit targetUnit = null;
			for (int i = 0; i < list.Count; i++)
			{
				if (Mathf.Abs(_myTrm.position.x - list[i].transform.position.x) >= targetRange)
				{
					continue;
				}
				if (_myUnit.ETeam.Equals(TeamType.MyTeam) && _myTrm.position.x > list[i].transform.position.x)
				{
					continue;
				}
				if (!_myUnit.ETeam.Equals(TeamType.MyTeam) && _myTrm.position.x < list[i].transform.position.x)
				{
					continue;
				}
				if (list[i]._isInvincibility)
				{
					continue;
				}

				targetUnit = list[i];
				targetRange = Vector2.Distance(_myTrm.position, targetUnit.transform.position);
			}

			if (targetUnit != null)
			{
				if (Mathf.Abs(_myTrm.position.x - targetUnit.transform.position.x) < _myUnit.UnitStat.Return_Range())
				{
					//사정거리에 상대가 있으면 공격
					CheckTargetUnit(targetUnit);
				}
			}
		}
	}

}