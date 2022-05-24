using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class FlyMoveState : AbstractMoveState
	{
		public override void Enter()
		{
			_myUnit.SetIsDontThrow(false);
			_curState = eState.MOVE;
			_curEvent = eEvent.ENTER;

			//�̵� �ִϸ��̼� 
			Animation(eState.MOVE);

			_curEvent = eEvent.UPDATE;
		}

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
				if (list[i].IsInvincibility)
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
					//�����Ÿ��� ��밡 ������ ����
					CheckTargetUnit(targetUnit);
				}
			}
		}

		protected override void CheckTargetUnit(Unit targetUnit)
		{
			_myTrm.DOLocalMoveY(0, 0.3f).OnComplete(() =>
			{
				base.CheckTargetUnit(targetUnit);
			});
		}
	}

}