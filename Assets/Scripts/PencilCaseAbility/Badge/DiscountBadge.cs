using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class DiscountBadge : AbstractBadge
{
    public override void RunBadgeAbility()
    {
        if(_teamType == TeamType.MyTeam)
        {
            //_battleManager.CommandCard.AddOneCard();
        }
    }
}
