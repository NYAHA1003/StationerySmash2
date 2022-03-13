using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class PencilCase_Unit : Unit
{

    private PencilCaseDataSO pencilCaseData;
    public PencilCase_Ability_State pencilCase_Ability_State;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        Set_PencilCaseData(battleManager.pencilCaseDataSO, eTeam, battleManager);
        Set_Position();

        if (eTeam.Equals(TeamType.MyTeam))
        {
            battleManager.battle_Unit.Add_UnitListMy(this);
            return;
        }
        battleManager.battle_Unit.Add_UnitListEnemy(this);
        
    }

    private void Set_Position()
    {
        if (eTeam.Equals(TeamType.MyTeam))
        {
            transform.position = new Vector2(-battleManager.currentStageData.max_Range, 0);
            return;
        }
        transform.position = new Vector2(battleManager.currentStageData.max_Range, 0);
    }

    public void Set_PencilCaseData(PencilCaseDataSO pencilCaseData, TeamType eTeam, BattleManager battleManager)
    {//∆¿, ¿Ã∏ß º≥¡§
        this.eTeam = eTeam;
        transform.name = pencilCaseData.dataBase.card_Name + this.eTeam;
        switch (this.eTeam)
        {
            case TeamType.Null:
                throw new System.Exception("∆¿ ø°∑Ø");
            case TeamType.MyTeam:
                spr.color = Color.red;
                break;
            case TeamType.EnemyTeam:
                spr.color = Color.blue;
                break;
        }
        this.pencilCaseData = pencilCaseData;

        switch (this.pencilCaseData.pencilCaseType)
        {
            default:
            case PencilCaseType.Normal:
                pencilCase_Ability_State = new PencilCase_Normal_Ability_State(battleManager, this.pencilCaseData.pencilCaseType);
                break;
        }

        spr.sprite = pencilCaseData.dataBase.card_Sprite;
        this.battleManager = battleManager;
        hp = pencilCaseData.dataBase.unitData.unit_Hp;

        unitState = new PencilCase_Normal_State(transform, spr.transform, this);
        battleManager.battle_PenCase.Set_PencilCase(this, eTeam);
        
        isSettingEnd = true;

    }

    public override void Run_Damaged(AtkData atkData)
    {
        unitState = new PencilCase_Normal_Damaged_State(transform, spr.transform, this, atkData);
    }

    public override void Add_StatusEffect(AtkType atkType, params float[] value)
    {

    }
}
