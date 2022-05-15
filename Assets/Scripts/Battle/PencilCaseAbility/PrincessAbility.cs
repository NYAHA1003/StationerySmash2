using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
namespace Battle.PCAbility
{
    public class PrincessAbility : AbstractPencilCaseAbility
    {
        int count = 0;
        public override void RunPencilCaseAbility()
        {
            AtkData hillData = new AtkData(_battleManager.PencilCaseComponent.PlayerPencilCase, -100, 0, 0, 0, true, 15200 + count, EffAttackType.Normal, EffectType.Attack);
            for (int i = 0; i < _battleManager.UnitComponent._playerUnitList.Count; i++)
            {
                Unit unit = _battleManager.UnitComponent._playerUnitList[i];
                unit.Run_Damaged(hillData);
            }

        }
        public override bool AIAbilityCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}