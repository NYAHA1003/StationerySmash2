using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public abstract class AbstractMoveState : AbstractUnitState
	{

		public override void Enter()
		{
			_curState = eState.MOVE;
			_curEvent = eEvent.ENTER;

			_myUnit.SetIsDontThrow(false);

			//스티커 사용
			_myUnit.UnitSticker.RunMoveStickerAbility(_curState);

			//이동 애니메이션 시작
			Animation(eState.MOVE);

			base.Enter();
		}

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
		protected virtual void MoveMyTeam()
		{
			if (_myTrm.transform.position.x < _stateManager.GetStageData().max_Range - 0.1f)
			{
				_myTrm.Translate(Vector2.right * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
			}
		}

		/// <summary>
		/// 적 팀의 유닛 이동
		/// </summary>
		protected virtual void MoveEnemyTeam()
		{
			if (_myTrm.transform.position.x > -_stateManager.GetStageData().max_Range + 0.1f)
			{
				_myTrm.Translate(Vector2.left * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
			}
		}

		/// <summary>
		/// 상대 유닛이 사정거리에 있는지 체크
		/// </summary>
		/// <param name="list"></param>
		protected virtual void CheckRange(List<Unit> list)
		{
			Unit targetUnit = null;
			int firstNum = 0;
			int lastNum = list.Count - 1;
			int currentIndex = 0;
			float posX = _myTrm.position.x;
			float posY = _myTrm.position.y;

			if (list.Count == 0)
			{
				return;
			}

			if (_myUnit.ETeam == TeamType.MyTeam)
			{
				if (posX < list[lastNum].transform.position.x)
				{
					targetUnit = list[lastNum];
					currentIndex = lastNum;
				}
			}
			else if (_myUnit.ETeam == TeamType.EnemyTeam)
			{
				if (posX > list[lastNum].transform.position.x)
				{
					targetUnit = list[lastNum];
					currentIndex = lastNum;
				}
			}

			if (targetUnit == null)
			{
				BinarySearch(list, posX, lastNum, firstNum, ref targetUnit, out currentIndex);
			}
			NextTargetUnit(list, posY, ref targetUnit, ref currentIndex);

			if (targetUnit != null)
			{
				if (Vector2.Distance(_myTrm.position, targetUnit.transform.position) < _myUnit.UnitStat.Return_Range())
				{
					//사정거리에 상대가 있으면 공격
					CheckTargetUnit(targetUnit);
				}
			}
		}


		/// <summary>
		/// 사정거리안에 적이 있다
		/// </summary>
		/// <param name="targetUnit"></param>
		protected virtual void CheckTargetUnit(Unit targetUnit)
		{
			_stateManager.Set_Attack(targetUnit);
		}

		/// <summary>
		/// 적 이진 탐색
		/// </summary>
		/// <param name="list"></param>
		/// <param name="posX"></param>
		/// <param name="lastNum"></param>
		/// <param name="firstNum"></param>
		/// <param name="targetUnit"></param>
		/// <param name="currentIndex"></param>
		private void BinarySearch(List<Unit> list, float posX, int lastNum, int firstNum, ref Unit targetUnit, out int currentIndex)
		{
			int loopnum = 0;
			float targetPosX = 0;
			while (true)
			{
				if (list.Count == 0)
				{
					currentIndex = 0;
				}

				int find = (lastNum + firstNum) / 2;
				targetPosX = list[find].transform.position.x;

				if (posX == targetPosX)
				{
					targetUnit = list[find];
					currentIndex = find;
					break;
				}

				if (_myUnit.ETeam == TeamType.MyTeam)
				{
					if (posX > targetPosX)
					{
						lastNum = find;
					}
					else if (posX < targetPosX)
					{
						firstNum = find;
					}
				}
				else if (_myUnit.ETeam == TeamType.EnemyTeam)
				{
					if (posX < targetPosX)
					{
						lastNum = find;
					}
					else if (posX > targetPosX)
					{
						firstNum = find;
					}
				}

				if (lastNum - firstNum <= 1)
				{
					targetUnit = list[firstNum];
					currentIndex = firstNum;
					break;
				}

				loopnum++;
				if (loopnum > 10000)
				{
					throw new System.Exception("Infinite Loop");
				}
			}
		}

		/// <summary>
		/// 공격할 수 없는 적일 때 다음 타겟 찾기
		/// </summary>
		/// <param name="list"></param>
		/// <param name="posY"></param>
		/// <param name="targetUnit"></param>
		/// <param name="currentIndex"></param>
		private void NextTargetUnit(List<Unit> list, float posY, ref Unit targetUnit, ref int currentIndex)
		{
			while (targetUnit._isInvincibility || targetUnit.transform.position.y > posY)
			{
				if (list.Count == 0)
				{
					return;
				}

				if (currentIndex - 1 < 0)
				{
					targetUnit = null;
					break;
				}
				targetUnit = list[--currentIndex];
			}
		}
	}
}
