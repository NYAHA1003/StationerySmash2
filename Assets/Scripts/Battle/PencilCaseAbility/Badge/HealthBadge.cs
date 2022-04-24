using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBadge : AbstractBadge
{
    public override void RunBadgeAbility()
    {
        _pencilCaseUnit.UnitStat.SetBonusMaxHP(100);
    }
}
