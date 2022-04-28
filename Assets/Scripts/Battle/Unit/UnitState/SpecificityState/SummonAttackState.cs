using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public class SummonAttackState : AbstractAttackState
	{
		/// <summary>
		/// 소환
		/// </summary>
		protected override void Attack()
		{
			//공격 애니메이션
			Animation();
			Summon();

			//공격 딜레이 초기화
			_currentdelay = 0;
			SetUnitDelayAndUI();

			//대기 상태로 돌아감
			_stateManager.Set_Wait(0.4f);
			_curEvent = eEvent.EXIT;
		}

		/// <summary>
		/// 유닛 소환
		/// </summary>
		protected virtual void Summon()
		{

		}
	}

}