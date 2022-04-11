using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class GoldAbility : AbstractPencilCaseAbility
{
    public override void RunPencilCaseAbility()
    {
        _battleManager.CommandCost.AddCost(1);
    }
    public override bool AIAbilityCondition()
    {
        //ÀûÀÌ ¸ר ¾¸
        return false;
    }
}
