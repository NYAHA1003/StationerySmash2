using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

namespace Battle.Units
{

	public abstract class AbstractWaitState : AbstractUnitState
	{
		private float _waitTime = 0.0f;
		private float _extraWaitTime = 0.0f;

		public override void Enter()
		{
			_curState = eState.WAIT;
			_curEvent = eEvent.ENTER;

			//스티커 사용
			_myUnit.UnitSticker.RunWaitStickerAbility(_curState);

			//애니메이션 제거
			ResetAllStateAnimation();

			base.Enter();
		}

		public override void Update()
		{
			WaitTimer();
		}
		/// <summary>
		/// 대기 시간을 설정함
		/// </summary>
		/// <param name="waitTime"></param>
		public void Set_Time(float waitTime)
		{
			this._waitTime = waitTime;
		}
		/// <summary>
		/// 추가 대기시간(스턴 등)의 시간을 설정함
		/// </summary>
		/// <param name="extraWaitTime"></param>
		public void Set_ExtraTime(float extraWaitTime)
		{
			this._extraWaitTime = extraWaitTime;
			this._waitTime = _waitTime + extraWaitTime;
		}


		/// <summary>
		/// 대기 유지시간
		/// </summary>
		protected void WaitTimer()
		{
			if (_waitTime > 0)
			{
				_waitTime -= Time.deltaTime;
				return;
			}

			//이동으로 스테이트 변경
			_stateManager.Set_Move();
		}
	}

}