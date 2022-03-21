using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Battle_PencilCase : BattleCommand
{
    public Unit pencilCase_My;
    public Unit pencilCase_Enemy;
    public PencilCaseData pencilCaseDataMy;
    public PencilCaseData pencilCaseDataEnemy;
    public PencilCase_Ability_State pencilCase_Ability_State_My;
    public PencilCase_Ability_State pencilCase_Ability_State_Enemy;
    public Battle_PencilCase(BattleManager battleManager, Unit pencilCase_My, Unit pencilCase_Enemy, PencilCaseData pencilCaseDataMy, PencilCaseData pencilCaseDataEnemy) : base(battleManager)
    {
        this.pencilCase_My = pencilCase_My;
        this.pencilCase_Enemy = pencilCase_Enemy;
        this.pencilCaseDataMy = pencilCaseDataMy;
        this.pencilCaseDataEnemy = pencilCaseDataEnemy;
        this.pencilCase_Ability_State_My = pencilCaseDataMy.pencilState;
        this.pencilCase_Ability_State_Enemy = pencilCaseDataEnemy.pencilState;
        this.battleManager = battleManager;

        pencilCase_My.Set_UnitData(pencilCaseDataMy.pencilCaseData, TeamType.MyTeam, battleManager, -1, 1);
        battleManager.unit_MyDatasTemp.Add(pencilCase_My);
        pencilCase_My.transform.position = new Vector2(-battleManager.currentStageData.max_Range, 0);
        Set_PencilCaseAbility(ref pencilCase_Ability_State_My, pencilCaseDataMy);
        
        pencilCase_Enemy.Set_UnitData(pencilCaseDataEnemy.pencilCaseData, TeamType.EnemyTeam, battleManager, -2, 1);
        battleManager.unit_EnemyDatasTemp.Add(pencilCase_Enemy);
        pencilCase_Enemy.transform.position = new Vector2(battleManager.currentStageData.max_Range, 0);
        Set_PencilCaseAbility(ref pencilCase_Ability_State_Enemy, pencilCaseDataEnemy);
    }

    public void Run_PencilCaseAbility()
    {
        pencilCase_Ability_State_My.Run_PencilCaseAility();
    }

    public void Set_PencilCaseAbility(ref PencilCase_Ability_State ability_State, PencilCaseData pencilCaseData)
    {
        switch (pencilCaseData.pencilCaseType)
        {
            default:
            case PencilCaseType.Normal:
                ability_State = new PencilCase_Normal_Ability_State(battleManager);
                break;
        }
    }
}
