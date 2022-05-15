using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.PCAbility
{

    public class SoccerAbility : AbstractPencilCaseAbility
    {
        float speed = 10;
        float duration = 4;
        public override void RunPencilCaseAbility()
        {
            _battleManager.CostComponent.AddCostSpeed(speed);
            Wait();
            _battleManager.CostComponent.AddCostSpeed(-speed);
        }
        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(duration);
        }
        public override bool AIAbilityCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}