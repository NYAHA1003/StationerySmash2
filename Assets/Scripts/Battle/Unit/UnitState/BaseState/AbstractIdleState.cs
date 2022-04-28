using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;


namespace Battle.Units
{

	public abstract class AbstractIdleState : AbstractUnitState
	{
		public override void Enter()
		{
			_curState = eState.IDLE;
			_curEvent = eEvent.ENTER;

			//스티커 사용
			_myUnit.UnitSticker.RunIdleStickerAbility(_curState);

			//소환시 애니메이션
			Animation();

			//대기상태로 만든다
			IdleToWaitTime();
		}

		/// <summary>
		/// 대기 상태로 넘어갔을 때 대기 시간
		/// </summary>
		protected virtual void IdleToWaitTime()
		{
			//대기 상태로 만든다
			_stateManager.Set_Wait(0.5f);
		}
	}

}