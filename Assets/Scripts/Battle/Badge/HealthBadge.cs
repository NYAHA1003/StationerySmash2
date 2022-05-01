using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Badge
{

public class HealthBadge : AbstractBadge
{
    public override void RunBadgeAbility()
    {
        _pencilCaseUnit.UnitStat.SetBonusMaxHP(100);
    }
    }
}
