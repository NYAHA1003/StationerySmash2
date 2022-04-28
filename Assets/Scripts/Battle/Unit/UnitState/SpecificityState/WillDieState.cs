using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{


	public abstract class WillDieState : AbstractDieState
	{
		public override void Enter()
		{
			//�����̹� ���� UI �� ���̰� �ϰ� �����̻� ����
			_myUnit.UnitStateEff.DeleteEffStetes();
			_myUnit.SetIsDontThrow(true);
			_myUnit.UnitSprite.ShowUI(false);

			//����
			_myUnit.SetIsInvincibility(true);
			_myTrm.DOKill();
			_mySprTrm.DOKill();

			//���� ȿ�� �ߵ�
			Will();

			_curEvent = eEvent.UPDATE;
		}

		/// <summary>
		/// ���� ȿ�� �ߵ�
		/// </summary>
		protected virtual void Will()
		{
			//�������� �״� �ִϸ��̼� ���
			RandomDieAnimation();
		}
	}

}