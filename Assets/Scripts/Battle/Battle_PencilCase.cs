using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Battle_PencilCase : BattleCommand
{
    public PencilCase_Unit pencilCase_My;
    public PencilCase_Unit pencilCase_Enemy;
    public PencilCase_Ability_State pencilCase_Ability_State_My;
    public PencilCase_Ability_State pencilCase_Ability_State_Enemy;
    public Battle_PencilCase(BattleManager battleManager) : base(battleManager)
    {
    }

    public void Set_PencilCase(PencilCase_Unit pencilCase_Unit, TeamType eteam)
    {
        if(eteam.Equals(TeamType.MyTeam))
        {
            pencilCase_My = pencilCase_Unit;
            pencilCase_Ability_State_My = pencilCase_Unit.pencilCase_Ability_State;
            return;
        }

        pencilCase_My = pencilCase_Unit;
        pencilCase_Ability_State_Enemy = pencilCase_Unit.pencilCase_Ability_State;
    }

    public void Run_PencilCaseAbility()
    {
        pencilCase_Ability_State_My.Run_PencilCaseAility();
    }
}
