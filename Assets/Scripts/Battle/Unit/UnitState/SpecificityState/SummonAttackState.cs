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
		/// ��ȯ
		/// </summary>
		protected override void Attack()
		{
			//���� �ִϸ��̼�
			Animation();
			Summon();

			//���� ������ �ʱ�ȭ
			_currentdelay = 0;
			SetUnitDelayAndUI();

			//��� ���·� ���ư�
			_stateManager.Set_Wait(0.4f);
			_curEvent = eEvent.EXIT;
		}

		/// <summary>
		/// ���� ��ȯ
		/// </summary>
		protected virtual void Summon()
		{

		}
	}

}