using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;

namespace Battle.Units
{

	public class PcKillAttackState : AbstractAttackState
	{
		protected override void SetAttackData(ref AtkData atkData)
		{
			base.SetAttackData(ref atkData);
			atkData.Reset_Type(EffAttackType.PCKill);
		}
	}

}
