using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle.Badge
{

	public class InvincibleBadge : AbstractBadge
	{
		Unit unit;
		public override void RunBadgeAbility()
		{
			while (true)
			{
				_pencilCaseUnit.UnitStat.Invincible();
				Wait();
			}
		}
		private IEnumerator Wait()
		{
			yield return new WaitForSeconds(60f);
		}
	}

}