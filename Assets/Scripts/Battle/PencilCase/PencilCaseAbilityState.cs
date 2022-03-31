using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class PencilCaseAbilityState
{
    public BattleManager battleManager;
    public PencilCaseAbilityState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public abstract void RunPencilCaseAility();
}

public class PencilCaseNormalAbilityState : PencilCaseAbilityState
{
    public PencilCaseNormalAbilityState(BattleManager battleManager) : base(battleManager)
    {
    }

    /// <summary>
    /// ī�带 ��� �����Ѵ�
    /// </summary>
    public override void RunPencilCaseAility()
    {
        battleManager.CommandCard.ClearCards();
    }
}
