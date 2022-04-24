using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackBadge : AbstractBadge
{
    public override void RunBadgeAbility()
    {
        _pencilCaseUnit.UnitStat.SetBonusMaxHPPercent(15);
        _battleManager.CommandCost.AddCostSpeed(10);
    }
}
