using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle;
namespace Battle.Badge
{

	public class Magnet_SBadge : AbstractBadge
	{
		int num = -1;
		public override void SetBattleManager(BattleManager battleManager)
		{
			base.SetBattleManager(battleManager);
		}

		public override void SetBadge(PencilCaseComponent pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
		{
			base.SetBadge(pencilCaseCommand, pencilCaseUnit, teamType, badgeData);
		}

		public override void RunBadgeAbility()
		{
			//throw new System.NotImplementedException();
		}

		public void Magnet_S(BadgeData badgeData)
		{
			if (_pencilCaseCommand.FindBadge(_teamType, BadgeType.Magnet_N))
			{
				num = 1;
			}
			_pencilCaseUnit.UnitStat.SetBonusMaxHP(100 * num);
		}
	}

}