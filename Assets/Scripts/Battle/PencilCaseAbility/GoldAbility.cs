using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.PCAbility
{

public class GoldAbility : AbstractPencilCaseAbility
{
    public override void RunPencilCaseAbility()
    {
        _battleManager.CostComponent.AddCost(1);
    }
    public override bool AIAbilityCondition()
    {
        //���� �� ��
        return false;
    }
}

}