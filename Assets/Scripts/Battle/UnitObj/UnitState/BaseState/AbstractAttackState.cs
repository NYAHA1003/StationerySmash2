using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{
	
	public abstract class AbstractAttackState : AbstractUnitState
	{

		protected Unit _targetUnit = null; //공격할 유닛
		protected float _currentdelay = 0; //현재 딜레이
		protected float _maxdelay = 100; //끝 딜레이
		private bool isAttacked; //공격 중인지

		public override void Enter()
		{
			isAttacked = false;
			_curState = eState.ATTACK;
			_curEvent = eEvent.ENTER;
			Animation(eState.WAIT);

			//공격 딜레이를 유닛의 딜레이로 설정
			_currentdelay = _myUnit.UnitStat.AttackDelay;

			base.Enter();
		}
		public override void Update()
		{
			if (!isAttacked)
			{
				//상대와의 거리 체크
				CheckRangeToTarget();

				//쿨타임 감소
				if (AttackDelay())
				{
					Attack();
				}
			}
		}

		/// <summary>
		/// 공격할 유닛 설정
		/// </summary>
		/// <param name="targetUnit"></param>
		public void Set_Target(Unit targetUnit)
		{
			this._targetUnit = targetUnit;
		}


		/// <summary>
		/// 공격
		/// </summary>
		protected virtual void Attack()
		{
			isAttacked = true;

			//공격 애니메이션
			Animation(eState.ATTACK);
			_stateManager.Set_Wait(0.5f);

			//공격 딜레이 초기화
			_currentdelay = 0;
			SetUnitDelayAndUI();


			//공격 명중률에 따라 미스가 뜬다.
			if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy())
			{
				AtkData atkData = null;
				SetAttackData(ref atkData);

				//스티커 사용
				_myUnit.UnitSticker.RunAttackStickerAbility(_curState, ref atkData);

				//공격 유형에 따라 공격
				switch (_myUnitData.attackType)
				{
					case AttackType.Normal:
						NormalAttack(atkData);
						break;
					case AttackType.Range:
						if (_myUnit.ETeam == TeamType.MyTeam)
						{
							RangeAttack(atkData, _myUnit.BattleManager.UnitComponent._enemyUnitList);
						}
						else if (_myUnit.ETeam == TeamType.EnemyTeam)
						{
							RangeAttack(atkData, _myUnit.BattleManager.UnitComponent._playerUnitList);
						}
						break;
				}
				_targetUnit = null;
				return;
			}
			else
			{
				Debug.Log("미스");
			}
		}

		/// <summary>
		/// 노말 공격
		/// </summary>
		protected void NormalAttack(AtkData atkData)
		{
			_targetUnit.Run_Damaged(atkData);
		}

		/// <summary>
		/// 범위 공격
		/// </summary>
		protected void RangeAttack(AtkData atkData, List<Unit> list)
		{
			for (int targetIndex = _targetUnit.OrderIndex; targetIndex >= 0; targetIndex--)
			{
				if (list[targetIndex]._isInvincibility || list[targetIndex].transform.position.y > _myTrm.transform.position.y)
				{
					continue;
				}

				if (Vector2.Distance(_myTrm.position, list[targetIndex].transform.position) < _myUnit.UnitStat.Return_Range())
				{
					list[targetIndex].Run_Damaged(atkData);
				}
				else
				{
					break;
				}
			}
		}

		/// <summary>
		/// 유닛의 딜레이랑, 딜레이바 UI 수정
		/// </summary>
		protected void SetUnitDelayAndUI()
		{
			_myUnit.UnitSprite.UpdateDelayBar(_currentdelay / _maxdelay);
			_myUnit.UnitStat.SetAttackDelay(_currentdelay);
		}

		/// <summary>
		/// 타겟과의 거리 체크
		/// </summary>
		protected virtual void CheckRangeToTarget()
		{
			if (_targetUnit == null)
			{
				_stateManager.Set_Move();
				return;
			}
			if (_targetUnit._isInvincibility)
			{
				_stateManager.Set_Move();
				return;
			}
			if (Vector2.Distance(_myTrm.position, _targetUnit.transform.position) > _myUnit.UnitStat.Return_Range())
			{
				_stateManager.Set_Move();
				return;
			}
			if (_myUnit.ETeam == TeamType.MyTeam && _myTrm.position.x > _targetUnit.transform.position.x)
			{
				_stateManager.Set_Move();
				return;
			}
			if (_myUnit.ETeam == TeamType.EnemyTeam && _myTrm.position.x < _targetUnit.transform.position.x)
			{
				_stateManager.Set_Move();
				return;
			}
			if (_targetUnit.transform.position.y > _myTrm.position.y)
			{
				_stateManager.Set_Move();
				return;
			}
		}

		/// <summary>
		/// 공격 데이터 설정
		/// </summary>
		/// <param name="atkData"></param>
		protected virtual void SetAttackData(ref AtkData atkData)
		{
			atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, EffAttackType.Normal, _myUnit.SkinData._effectType, originValue);
		}

		/// <summary>
		/// 공격할 수 있을 때 까지 대기한다
		/// </summary>
		/// <returns>True면 공격, 아니면 딜레이</returns>
		private bool AttackDelay()
		{
			if (_maxdelay >= _currentdelay)
			{
				_currentdelay += _myUnit.UnitStat.Return_AttackSpeed() * Time.deltaTime;
				SetUnitDelayAndUI();
				return false;
			}
			return true;
		}

	}

}