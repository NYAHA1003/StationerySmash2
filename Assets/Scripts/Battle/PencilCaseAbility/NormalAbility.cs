using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.PCAbility
{
    public class NormalAbility : AbstractPencilCaseAbility
    {
        /// <summary>
        /// ī�带 ��� �����Ѵ�
        /// </summary>
        public override void RunPencilCaseAbility()
        {
            _battleManager.CardComponent.ClearCards();
        }
        public override bool AIAbilityCondition()
        {
            //���� �� ��
            return false;
        }
    }
}