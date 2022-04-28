using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

namespace Battle.Units
{

	public class CritAttackState : AbstractAttackState
	{
		protected override void SetAttackData(ref AtkData atkData)
		{
			base.SetAttackData(ref atkData);
			if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy() / 10)
			{
				//크리티컬
				atkData.Reset_Damage(atkData.damage * 2);
			}
			else
			{
				atkData.Reset_Damage(atkData.damage);
			}
		}
	}

}