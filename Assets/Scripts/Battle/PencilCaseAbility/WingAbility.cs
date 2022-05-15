using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
namespace Battle.PCAbility
{


    public class WingAbility : AbstractPencilCaseAbility
    {
        int count = 0;
        public override void RunPencilCaseAbility()
        {
            AtkData atkData = new AtkData(_battleManager.PencilCaseComponent.PlayerPencilCase, 0, 10, 0, 45, true, 13200 + count, EffAttackType.Normal, EffectType.Attack);

            for (int i = 0; i < _battleManager.UnitComponent._enemyUnitList.Count; i++)
            {
                Unit unit = _battleManager.UnitComponent._enemyUnitList[i];
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