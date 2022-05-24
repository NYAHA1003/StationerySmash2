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
			//팀에 따른 이동 및 사정거리 체크
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
		/// 플레이어 팀의 유닛 이동
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
		/// 적 팀의 유닛 이동
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
		/// 스테이지 끝에 도착했을 때 사용하는 함수
		/// </summary>
		protected virtual void ArrivalStageRange()
		{
			//유닛 삭제
			ResetSprTrm();
			_curEvent = eEvent.EXIT;
			_myUnit.Delete_Unit();
		}
	}

}