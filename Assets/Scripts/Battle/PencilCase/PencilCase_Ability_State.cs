using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class PencilCase_Ability_State
{
    public BattleManager battleManager;
    public PencilCase_Ability_State(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public abstract void Run_PencilCaseAility();
}

public class PencilCase_Normal_Ability_State : PencilCase_Ability_State
{
    public PencilCase_Normal_Ability_State(BattleManager battleManager) : base(battleManager)
    {
    }

    /// <summary>
    /// 카드를 모두 제거한다
    /// </summary>
    public override void Run_PencilCaseAility()
    {
        battleManager.battle_Card.Clear_Cards();
    }
}
