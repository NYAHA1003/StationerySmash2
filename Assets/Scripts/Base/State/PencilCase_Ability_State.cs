using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class PencilCase_Ability_State
{
    public BattleManager battleManager;
    public PencilCaseType pencilCaseType;
    public PencilCase_Ability_State(BattleManager battleManager, PencilCaseType pencilCaseType)
    {
        this.battleManager = battleManager;
        this.pencilCaseType = pencilCaseType;
    }

    public abstract void Run_PencilCaseAility();
}

public class PencilCase_Normal_Ability_State : PencilCase_Ability_State
{
    public PencilCase_Normal_Ability_State(BattleManager battleManager, PencilCaseType pencilCaseType) : base(battleManager, pencilCaseType)
    {
    }

    /// <summary>
    /// 카드를 랜덤으로 다시 뽑는다
    /// </summary>
    public override void Run_PencilCaseAility()
    {
        battleManager.battle_Card.Clear_Cards();
        battleManager.battle_Card.Add_AllCard();
    }
}
