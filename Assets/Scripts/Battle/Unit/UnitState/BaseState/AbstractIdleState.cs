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

			//��ƼĿ ���
			_myUnit.UnitSticker.RunIdleStickerAbility(_curState);

			//��ȯ�� �ִϸ��̼�
			Animation();

			//�����·� �����
			IdleToWaitTime();
		}

		/// <summary>
		/// ��� ���·� �Ѿ�� �� ��� �ð�
		/// </summary>
		protected virtual void IdleToWaitTime()
		{
			//��� ���·� �����
			_stateManager.Set_Wait(0.5f);
		}
	}

}