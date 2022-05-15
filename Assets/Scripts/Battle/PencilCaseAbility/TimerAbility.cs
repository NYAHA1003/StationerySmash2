using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
namespace Battle.PCAbility
{


    public class TimerAbility : AbstractPencilCaseAbility
    {
        public override void RunPencilCaseAbility()
        {
            for (int i = 0; i < _battleManager.UnitComponent._playerUnitList.Count; i++)
            {
                Unit unit = _battleManager.UnitComponent._playerUnitList[i];

                unit.AddStatusEffect(EffAttackType.Exch, 2, 1000, 1000);
            }
        }
        public override bool AIAbilityCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}