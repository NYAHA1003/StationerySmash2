using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDownBadge : AbstractBadge
{
    public override void RunBadgeAbility()
    {
        _battleManager.CommandTime.IncreaseTime(-30);
    }
}
