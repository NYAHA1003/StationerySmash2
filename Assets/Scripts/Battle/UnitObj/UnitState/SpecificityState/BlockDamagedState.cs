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

			//고유효과 속성이면 효과 적용
			if (_atkData.atkType > EffAttackType.Inherence)
			{
				_myUnit.AddInherence(_atkData);
			}

			//무적여부, 데미지 적용
			_myUnit.BattleManager.EffectComponent.SetEffect(_atkData._effectType, new EffData(_myTrm.transform.position, 0.2f));
			_myUnit.SubtractHP(_atkData.damage * (_myUnit.UnitStat.DamagedPercent / 100) - _myUnit.UnitStat.DamageDecrese); //여기

			//스티커 사용
			_myUnit.UnitSticker.RunDamagedStickerAbility(_curState, ref _atkData);

			//체력이 0 이하면 죽음 상태로 전환
			if (_myUnit.UnitStat.Hp <= 0)
			{
				_stateManager.Set_Die();
				return;
			}

			//넉백 적용
			KnockBack();
		}
		public override void KnockBack()
		{
			//자기 이하 무게의 넉백 무시
			if(_atkData.attacker.UnitStat.Return_Weight() > _myUnit.UnitStat.Return_Weight())
			{
				base.KnockBack();
			}
			else
			{
				_stateManager.Set_Wait(0.4f);
			}
		}
	}
}
