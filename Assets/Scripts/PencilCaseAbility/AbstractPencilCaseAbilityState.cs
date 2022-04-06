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
    /// ����ɷ� �ߵ�
    /// </summary>
    public abstract void RunPencilCaseAbility();
}