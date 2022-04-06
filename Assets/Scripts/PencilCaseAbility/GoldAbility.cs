using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAbility : AbstractPencilCaseAbility
{
    public override void RunPencilCaseAbility()
    {
        _battleManager.CommandCost.AddCost(1);
    }
}
