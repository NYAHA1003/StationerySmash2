using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle.Badge
{

	public class GrowingSeedBadge : AbstractBadge
	{
		public override void RunBadgeAbility()
		{
			while (true)
			{
				_pencilCaseUnit.UnitStat.RecoveryHPPercent(1);
				Wait();
			}
		}
		private IEnumerator Wait()
		{
			yield return new WaitForSeconds(1f);
		}
	}

}