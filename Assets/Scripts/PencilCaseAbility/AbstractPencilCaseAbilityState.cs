using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class AbstractPencilCaseAbilityState
{
    public BattleManager _battleManager;

    public void SetState(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    /// <summary>
    /// 필통능력 발동
    /// </summary>
    public abstract void RunPencilCaseAbility();
}