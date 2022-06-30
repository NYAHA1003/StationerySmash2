using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{
	public class BlockDamagedState : AbstractDamagedState
	{
		public override void Enter()
		{
			_curState = eState.DAMAGED;
			_curEvent = eEvent.ENTER;

			//����ȿ�� �Ӽ��̸� ȿ�� ����
			if (_atkData.atkType > EffAttackType.Inherence)
			{
				_myUnit.AddInherence(_atkData);
			}

			//��������, ������ ����
			_myUnit.SetIsInvincibility(true);
			_myUnit.BattleManager.EffectComponent.SetEffect(_atkData._effectType, new EffData(_myUnit, _myTrm.transform.position, 0.2f));
			_myUnit.SubtractHP(_atkData.damage * (_myUnit.UnitStat.DamagedPercent / 100) - _myUnit.UnitStat.DamageDecrese); //����

			//��ƼĿ ���
			_myUnit.UnitSticker.RunDamagedStickerAbility(_curState, ref _atkData);

			if(_atkData.attacker.UnitStateChanger.UnitState.CurState == eState.THROW)
			{
				AtkData atkDataMy = new AtkData(_myUnit, 0, 0, 0, 0, true, 0, EffAttackType.Normal);

				atkDataMy.Reset_Kncockback(20, 0, 45, _myUnit.ETeam == TeamType.MyTeam);
				atkDataMy.Reset_Type(EffAttackType.Normal);
				atkDataMy.Reset_Value(1);
				atkDataMy.Reset_Damage(0);
				_atkData.attacker.Run_Damaged(atkDataMy);
			}

			//ü���� 0 ���ϸ� ���� ���·� ��ȯ
			if (_myUnit.UnitStat.Hp <= 0)
			{
				_stateManager.Set_Die();
				return;
			}

			_stateManager.Set_Wait(0.4f);
		}

		public override void Exit()
		{
			base.Exit();
			_myUnit.SetIsInvincibility(false);
		}
	}
}
