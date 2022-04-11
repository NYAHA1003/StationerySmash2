using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

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
            _battleManager.CommandCard.AddDictionary(_battleManager.CommandCard.AddOneCard, FirstCardCostDown);
        }
    }

    public void FirstCardCostDown()
    {
        CardMove card = _battleManager.CommandCard.CardList[0];
        card.SetCost(card.OriginCardCost - 1);
    }
}
