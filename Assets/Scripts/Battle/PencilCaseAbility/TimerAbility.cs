using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.PCAbility
{


    public class TimerAbility : AbstractPencilCaseAbility
    {
        public override void RunPencilCaseAbility()
        {
            for (int i = 0; i < _battleManager.CommandUnit._playerUnitList.Count; i++)
            {
                Unit unit = _battleManager.CommandUnit._playerUnitList[i];

                unit.AddStatusEffect(Utill.EffAttackType.Exch, 2, 1000, 1000);
            }
        }
        public override bool AIAbilityCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}