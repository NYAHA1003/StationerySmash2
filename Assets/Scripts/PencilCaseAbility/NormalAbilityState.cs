using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NormalAbilityState : AbstractPencilCaseAbilityState
{
    /// <summary>
    /// 카드를 모두 제거한다
    /// </summary>
    public override void RunPencilCaseAbility()
    {
        _battleManager.CommandCard.ClearCards();
    }
}
