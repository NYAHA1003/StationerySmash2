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

		protected Unit _targetUnit = null; //������ ����
		protected float _currentdelay = 0; //���� ������
		protected float _maxdelay = 100; //�� ������
		private bool isAttacked; //���� ������

		public override void Enter()
		{
			isAttacked = false;
			_curState = eState.ATTACK;
			_curEvent = eEvent.ENTER;
			Animation(eState.WAIT);

			//���� �����̸� ������ �����̷� ����
			_currentdelay = _myUnit.UnitStat.AttackDelay;

			base.Enter();
		}
		public override void Update()
		{
			if (!isAttacked)
			{
				//������ �Ÿ� üũ
				CheckRangeToTarget();

				//��Ÿ�� ����
				if (AttackDelay())
				{
					Attack();
				}
			}
		}

		/// <summary>
		/// ������ ���� ����
		/// </summary>
		/// <param name="targetUnit"></param>
		public void Set_Target(Unit targetUnit)
		{
			this._targetUnit = targetUnit;
		}


		/// <summary>
		/// ����
		/// </summary>
		protected virtual void Attack()
		{
			isAttacked = true;

			//���� �ִϸ��̼�
			Animation(eState.ATTACK);
			_stateManager.Set_Wait(0.5f);

			//���� ������ �ʱ�ȭ
			_currentdelay = 0;
			SetUnitDelayAndUI();


			//���� ���߷��� ���� �̽��� ���.
			if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy())
			{
				AtkData atkData = null;
				SetAttackData(ref atkData);

				//��ƼĿ ���
				_myUnit.UnitSticker.RunAttackStickerAbility(_curState, ref atkData);

				//���� ������ ���� ����
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
				Debug.Log("�̽�");
			}
		}

		/// <summary>
		/// �븻 ����
		/// </summary>
		protected void NormalAttack(AtkData atkData)
		{
			_targetUnit.Run_Damaged(atkData);
		}

		/// <summary>
		/// ���� ����
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
		/// ������ �����̶�, �����̹� UI ����
		/// </summary>
		protected void SetUnitDelayAndUI()
		{
			_myUnit.UnitSprite.UpdateDelayBar(_currentdelay / _maxdelay);
			_myUnit.UnitStat.SetAttackDelay(_currentdelay);
		}

		/// <summary>
		/// Ÿ�ٰ��� �Ÿ� üũ
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
		/// ���� ������ ����
		/// </summary>
		/// <param name="atkData"></param>
		protected virtual void SetAttackData(ref AtkData atkData)
		{
			atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, EffAttackType.Normal, _myUnit.SkinData._effectType, originValue);
		}

		/// <summary>
		/// ������ �� ���� �� ���� ����Ѵ�
		/// </summary>
		/// <returns>True�� ����, �ƴϸ� ������</returns>
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