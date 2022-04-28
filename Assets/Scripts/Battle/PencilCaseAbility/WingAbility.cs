using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.PCAbility
{


    public class WingAbility : AbstractPencilCaseAbility
    {
        int count = 0;
        public override void RunPencilCaseAbility()
        {
            AtkData atkData = new AtkData(_battleManager.CommandPencilCase.PlayerPencilCase, 0, 10, 0, 45, true, 13200 + count, EffAttackType.Normal, EffectType.Attack);

            for (int i = 0; i < _battleManager.CommandUnit._enemyUnitList.Count; i++)
            {
                Unit unit = _battleManager.CommandUnit._enemyUnitList[i];
                unit.Run_Damaged(atkData);
            }
            count++;
        }
        public override bool AIAbilityCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}