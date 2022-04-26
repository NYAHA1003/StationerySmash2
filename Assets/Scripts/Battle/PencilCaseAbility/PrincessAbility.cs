using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class PrincessAbility : AbstractPencilCaseAbility
{
    int count = 0;
    public override void RunPencilCaseAbility()
    {
        AtkData hillData = new AtkData(_battleManager.CommandPencilCase.PlayerPencilCase, -100, 0, 0, 0, true, 15200 + count, EffAttackType.Normal, EffectType.Attack);
        for (int i = 0; i < _battleManager.CommandUnit._playerUnitList.Count; i++)
        {
            Unit unit = _battleManager.CommandUnit._playerUnitList[i];
            unit.Run_Damaged(hillData);
        }

    }
    public override bool AIAbilityCondition()
    {
        throw new System.NotImplementedException();
    }
}
