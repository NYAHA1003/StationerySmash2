using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle;

namespace Battle.Badge
{

	public class DiscountBadge : AbstractBadge
	{
		private bool isSet = false;

		public override void SetBattleManager(BattleManager battleManager)
		{
			base.SetBattleManager(battleManager);
		}

		public override void RunBadgeAbility()
		{
			if (_teamType == TeamType.MyTeam && !isSet)
			{
				isSet = true;
				_battleManager.CardComponent.AddDictionary(_battleManager.CardComponent.DrawOneCard, FirstCardCostDown);
			}
		}

		public void FirstCardCostDown()
		{
			CardMove card = _battleManager.CardComponent.CardList[0];
			card.SetCost(card.OriginCardCost - 1);
		}
	}
}
