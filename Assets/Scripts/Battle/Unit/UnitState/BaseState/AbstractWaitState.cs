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

			//��ƼĿ ���
			_myUnit.UnitSticker.RunWaitStickerAbility(_curState);

			//�ִϸ��̼� ����
			ResetAllStateAnimation();

			base.Enter();
		}

		public override void Update()
		{
			WaitTimer();
		}
		/// <summary>
		/// ��� �ð��� ������
		/// </summary>
		/// <param name="waitTime"></param>
		public void Set_Time(float waitTime)
		{
			this._waitTime = waitTime;
		}
		/// <summary>
		/// �߰� ���ð�(���� ��)�� �ð��� ������
		/// </summary>
		/// <param name="extraWaitTime"></param>
		public void Set_ExtraTime(float extraWaitTime)
		{
			this._extraWaitTime = extraWaitTime;
			this._waitTime = _waitTime + extraWaitTime;
		}


		/// <summary>
		/// ��� �����ð�
		/// </summary>
		protected void WaitTimer()
		{
			if (_waitTime > 0)
			{
				_waitTime -= Time.deltaTime;
				return;
			}

			//�̵����� ������Ʈ ����
			_stateManager.Set_Move();
		}
	}

}