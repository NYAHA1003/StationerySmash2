using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Badge
{

	public class TimeUpBadge : AbstractBadge
	{
		public override void RunBadgeAbility()
		{
			_battleManager.CommandTime.IncreaseTime(30);
		}
	}

}