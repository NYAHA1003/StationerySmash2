using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.PCAbility
{
    public class NormalAbility : AbstractPencilCaseAbility
    {
        /// <summary>
        /// 카드를 모두 제거한다
        /// </summary>
        public override void RunPencilCaseAbility()
        {
            _battleManager.CardComponent.ClearCards();
        }
        public override bool AIAbilityCondition()
        {
            //적이 못 씀
            return false;
        }
    }
}