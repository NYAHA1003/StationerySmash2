using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle.Badge
{

	public class IncreaseBadge : AbstractBadge
	{
		public override void RunBadgeAbility()
		{
			_battleManager.CommandCard.AddMaxCard(1);
		}
	}
}
