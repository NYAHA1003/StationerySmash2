using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class PencilCase_Unit : Unit
{

    private PencilCaseDataSO pencilCaseData;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        Set_PencilCaseData(battleManager.pencilCaseDataSO, eTeam, battleManager);
        Set_Position();

        if (eTeam == TeamType.MyTeam)
        {
            battleManager.battle_Unit.Add_UnitListMy(this);
            return;
        }
        battleManager.battle_Unit.Add_UnitListEnemy(this);
        
    }

    private void Set_Position()
    {
        if (eTeam == TeamType.MyTeam)
        {
            transform.position = new Vector2(-battleManager.currentStageData.max_Range, 0);
            return;
        }
        transform.position = new Vector2(battleManager.currentStageData.max_Range, 0);
    }

    public void Set_PencilCaseData(PencilCaseDataSO pencilCaseData, TeamType eTeam, BattleManager battleManager)
    {//∆¿, ¿Ã∏ß º≥¡§
        this.eTeam = eTeam;
        transform.name = pencilCaseData.data.unitName + this.eTeam;
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
        spr.sprite = pencilCaseData.data.sprite;
        this.battleManager = battleManager;
        hp = pencilCaseData.data.hp;

        unitState = new PencilCase_Normal_State(transform, spr.transform, this);
        isSettingEnd = true;
    }

    public override void Run_Damaged(AtkData atkData)
    {
        unitState = new PencilCase_Normal_Damaged_State(transform, spr.transform, this, atkData);
    }
}
