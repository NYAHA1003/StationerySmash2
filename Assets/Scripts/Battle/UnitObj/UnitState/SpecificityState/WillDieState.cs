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
			//유언 효과 발동
			Will();

			base.Enter();
		}

		/// <summary>
		/// 유언 효과 발동
		/// </summary>
		protected virtual void Will()
		{
		}
	}

}