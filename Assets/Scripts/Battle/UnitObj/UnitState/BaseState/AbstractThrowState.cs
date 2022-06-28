using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public abstract class AbstractThrowState : AbstractUnitState
	{
		protected Vector2 _mousePos = Vector2.zero; // ���콺�� ���� ��ġ
		protected int _damageId;

		public override void Enter()
		{
			_curState = eState.THROW;
			_curEvent = eEvent.ENTER;

			//�� �����°� ����
			_stateManager.SetAnimation(eState.THROW);
			_myUnit.SetIsDontThrow(true);

			//��ƼĿ ���
			_myUnit.UnitSticker.RunThrowStickerAbility(_curState);

			//���� ������
			ThrowingUnit();

			//������ID ����
			_myUnit.DamageCount++;
			_damageId = _myUnit.MyUnitId * 10000 + _myUnit.DamageCount;


		}
		public override void Update()
		{
			//�������� ���� ��Ҵ��� üũ
			if (CheckWall())
			{
				EndThrow();
			}

			if (_myUnit.ETeam == TeamType.MyTeam)
			{
				CheckCollide(_myUnit.BattleManager.UnitComponent._enemyUnitList, 0.1f, false);
				return;
			}
			if (_myUnit.ETeam == TeamType.EnemyTeam)
			{
				CheckCollide(_myUnit.BattleManager.UnitComponent._playerUnitList, 0.1f, false);
				return;
			}
		}


		/// <summary>
		/// ���� ���������̶� �ε�ġ���� üũ
		/// </summary>
		/// <param name="list"></param>
		private void CheckCollide(List<Unit> list, float dis, bool isGround)
		{
			Unit targetUnit = null;
			for (int i = 0; i < list.Count; i++)
			{
				targetUnit = list[i];
				if (targetUnit.IsInvincibility)
				{
					continue;
				}
				float distance = UnitCollider.FindDistanceBetweenSegments(_myUnit.CollideData.GetPoint(_myTrm.position, _myUnit.Multiple), targetUnit.CollideData.GetPoint(targetUnit.transform.position, targetUnit.Multiple));
				if (distance < dis)
				{
					EndThrow();
					ThrowAttack(targetUnit, isGround);
				}
			}
		}

		/// <summary>
		/// ���콺�� ���� ��ġ ����
		/// </summary>
		/// <param name="pos"></param>
		public void SetThrowPos(Vector2 pos)
		{
			_mousePos = pos;
		}

		/// <summary>
		/// ���� ������
		/// </summary>
		private void ThrowingUnit()
		{
			//�������� ���� ����
			Vector2 direction = (Vector2)_myTrm.position - _mousePos;
			float dir = Mathf.Atan2(direction.y, direction.x);
			float dirx = Mathf.Atan2(direction.y, -direction.x);

			//������ �Ʒ����� ���ϸ� �����⸦ �����
			if (dir < 0)
			{
				_stateManager.Set_Wait(0.5f);
				_curEvent = eEvent.EXIT;
				return;
			}
			//�ʱ� ����
			float force = Mathf.Clamp(Vector2.Distance(_myTrm.position, _mousePos), 0, 1) * 4 * (100.0f / _myUnit.UnitStat.Return_Weight());
			//�ְ���
			float height = Parabola.Caculated_Height(force, dirx);
			//���� ���� �Ÿ�
			float width = Parabola.Caculated_Width(force, dirx);
			//���� ���� �ð�
			float time = Parabola.Caculated_Time(force, dir, 3);

			_curEvent = eEvent.UPDATE;

			SetKnockBack(_myTrm.DOJump(new Vector3(_myTrm.position.x - width, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
			{
				EndThrow();
				//��� �����̶� �ε�ġ���� üũ
				if (_myUnit.ETeam == TeamType.MyTeam)
				{
					CheckCollide(_myUnit.BattleManager.UnitComponent._enemyUnitList, 0.3f, true);
				}
				if (_myUnit.ETeam == TeamType.EnemyTeam)
				{
					CheckCollide(_myUnit.BattleManager.UnitComponent._playerUnitList, 0.3f, true);
				}
				//���� ������ ��� ���·� ���ư�
				_stateManager.Set_Wait(0.5f);
			}).SetEase(Parabola.Return_ParabolaCurve()));

		}

		/// <summary>
		/// �ε��� ���ֿ��� ������ �������� ����
		/// </summary>
		/// <param name="targetUnit"></param>
		protected virtual void ThrowAttack(Unit targetUnit, bool isGround)
		{
			//ī�޶� ��鸲
			_myUnit.BattleManager.CameraComponent.CameraInShake(_myTrm, 0.1f, 0.3f);

			float dir = Vector2.Angle((Vector2)_myTrm.position, (Vector2)targetUnit.transform.position);
			float extraKnockBack = (targetUnit.UnitStat.Return_Weight() - _myUnit.UnitStat.Return_Weight() * (float)targetUnit.UnitStat.Hp / targetUnit.UnitStat.MaxHp) * 0.025f;
			AtkData atkData = new AtkData(_myUnit, 0, 0, 0, 0, true, _damageId, EffAttackType.Normal, EffectType.Throw);
			
			UnitStat.WeightGrade unitWeightGrade = _myUnit.UnitStat.ReturnWeightGrade();
			UnitStat.WeightGrade targetWeightGrade = targetUnit.UnitStat.ReturnWeightGrade();
			
			if (targetUnit.UnitData._unitType == UnitType.PencilCase)
			{
				WeightLose(ref atkData, ref targetUnit, ref dir, ref extraKnockBack);
			}
			else
			{
				WeightWin(ref atkData, ref targetUnit, ref dir, ref extraKnockBack);
			}

		}

		/// <summary>
		/// ���԰� ���� ����� ������ ����
		/// </summary>
		/// <param name="targetUnit"></param>
		/// <param name="dir"></param>
		/// <param name="extraKnockBack"></param>
		private void WeightWin(ref AtkData atkData, ref Unit targetUnit, ref float dir, ref float extraKnockBack)
		{
			AtkData atkDataMy = new AtkData(_myUnit, 0, 0, 0, 0, true, _damageId, EffAttackType.Normal);

			//�ʱⵥ���� ����
			SetThrowAttackDamage(ref atkData, targetUnit);

			atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
			atkData.Reset_Type(EffAttackType.Stun);
			atkData.Reset_Value(((float)_myUnit.UnitStat.Return_Weight() / 100) + 1.5f);
			targetUnit.Run_Damaged(atkData);

			return;
		}

		/// <summary>
		/// ���԰� �� ����� ������ ����
		/// </summary>
		/// <param name="targetUnit"></param>
		/// <param name="dir"></param>
		/// <param name="extraKnockBack"></param>
		private void WeightLose(ref AtkData atkData, ref Unit targetUnit, ref float dir, ref float extraKnockBack)
		{
			AtkData atkDataMy = new AtkData(_myUnit, 0, 0, 0, 0, true, _damageId, EffAttackType.Normal);

			//�ʱⵥ���� ����
			SetThrowAttackDamage(ref atkData, targetUnit);

			atkData.Reset_Kncockback(15, extraKnockBack, dir, false);
			atkData.Reset_Type(EffAttackType.Stun);
			atkData.Reset_Value(((float)_myUnit.UnitStat.Return_Weight() / 100) + 1.5f);
			targetUnit.Run_Damaged(atkData);


			atkDataMy.Reset_Kncockback(10, 0, dir, true);
			atkDataMy.Reset_Type(EffAttackType.Normal);
			atkDataMy.Reset_Value(1);
			atkDataMy.Reset_Damage(0);
			_myUnit.Run_Damaged(atkDataMy);

			return;
		}

		/// <summary>
		/// ���� �������� �ʱ� �������� ����
		/// </summary>
		/// <param name="atkData"></param>
		/// <param name="targetUnit"></param>
		private void SetThrowAttackDamage(ref AtkData atkData, Unit targetUnit)
		{

			if (_myUnit.UnitStat.Return_Weight() > targetUnit.UnitStat.Return_Weight())
			{
				atkData.Reset_Damage(100 + (Mathf.RoundToInt((float)_myUnit.UnitStat.Return_Weight() - targetUnit.UnitStat.Return_Weight()) / 2));
			}
			else
			{
				atkData.Reset_Damage(100 + Mathf.RoundToInt((float)(targetUnit.UnitStat.Return_Weight() - _myUnit.UnitStat.Return_Weight()) / 5));
			}
		}

		/// <summary>
		/// �����Ⱑ ����
		/// </summary>
		private void EndThrow()
		{
			_myUnit.SetIsDontThrow(false);
			_myUnit.BattleManager.ThrowComponent.EndThrowUnit(_myUnit);
		}
	}

}