using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle.Badge
{
	public class SnackBadge : AbstractBadge
	{
		public override void RunBadgeAbility()
		{
			_pencilCaseUnit.UnitStat.SetBonusMaxHPPercent(15);
			_battleManager.CostComponent.AddCostSpeed(10);
		}
	}
}
