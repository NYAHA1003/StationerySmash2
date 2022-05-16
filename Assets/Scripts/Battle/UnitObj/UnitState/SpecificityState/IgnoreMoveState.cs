using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class IgnoreMoveState : AbstractMoveState
	{
		public override void Update()
		{
			//���� ���� �̵� �� �����Ÿ� üũ
			switch (_myUnit.ETeam)
			{
				case TeamType.Null:
					break;
				case TeamType.MyTeam:
					MoveMyTeam();
					CheckRange(_myUnit.BattleManager.UnitComponent._enemyUnitList);
					return;
				case TeamType.EnemyTeam:
					MoveEnemyTeam();
					CheckRange(_myUnit.BattleManager.UnitComponent._playerUnitList);
					return;
			}
		}

		/// <summary>
		/// �÷��̾� ���� ���� �̵�
		/// </summary>
		protected override void MoveMyTeam()
		{
			if (_myTrm.transform.position.x < _stateManager.GetStageData().max_Range - 0.1f)
			{
				_myTrm.Translate(Vector2.right * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
			}
			else
			{
				ArrivalStageRange();
			}
		}

		/// <summary>
		/// �� ���� ���� �̵�
		/// </summary>
		protected override void MoveEnemyTeam()
		{
			if (_myTrm.transform.position.x > -_stateManager.GetStageData().max_Range + 0.1f)
			{
				_myTrm.Translate(Vector2.left * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
			}
			else
			{
				ArrivalStageRange();
			}
		}

		/// <summary>
		/// �������� ���� �������� �� ����ϴ� �Լ�
		/// </summary>
		protected virtual void ArrivalStageRange()
		{
			//���� ����
			ResetSprTrm();
			_curEvent = eEvent.EXIT;
			_myUnit.Delete_Unit();
		}
	}

}