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
			//���� ȿ�� �ߵ�
			Will();

			base.Enter();
		}

		/// <summary>
		/// ���� ȿ�� �ߵ�
		/// </summary>
		protected virtual void Will()
		{
		}
	}

}