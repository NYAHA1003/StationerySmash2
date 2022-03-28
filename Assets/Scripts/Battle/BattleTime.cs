using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;


public class BattleTime : BattleCommand
{

    private StageData stageData;
    private float timer;
    private TextMeshProUGUI timeText;
    private bool isSuddenDeath;
    private bool isFinallyEnd;

    public BattleTime(BattleManager battleManager, TextMeshProUGUI timeText) : base(battleManager)
    {
        stageData = battleManager.CurrentStageData;
        timer = stageData.timeValue;
        this.timeText = timeText;
    }

    public void Update_Time()
    {
        if (isFinallyEnd) return;

        if (stageData.timeType == TimeType.DisabledTime)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timeText.text = string.Format("{0}:{1:D2}", (int)timer / 60, (int)timer % 60);
            return;
        }

        Set_SuddenDeath();
    }

    public void Set_SuddenDeath()
    {
        battleManager.BattleCard.Clear_Cards();
        battleManager.BattleUnit.Clear_Unit();

        if (!isSuddenDeath)
        {
            battleManager.BattleCard.Set_MaxCard(8);
            battleManager.BattleCost.Set_CostSpeed(500);
            isSuddenDeath = true;
            timer = 60;

            Debug.Log("서든데스 시작");
            return;
        }

        //체력 비교
        if(battleManager._myUnitDatasTemp[0].hp > battleManager._enemyUnitDatasTemp[0].hp)
        {
            Debug.Log("서든데스 승리");
            isFinallyEnd = true;
            return;
        }
        if (battleManager._myUnitDatasTemp[0].hp < battleManager._enemyUnitDatasTemp[0].hp)
        {
            Debug.Log("서든데스 패배");
            isFinallyEnd = true;
            return;
        }

        Debug.Log("서든데스 무승부");
        isFinallyEnd = true;
    }
}
