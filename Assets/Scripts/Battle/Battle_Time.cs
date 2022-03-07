using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

public class Battle_Time : BattleCommand
{
    private StageData stageData;
    private float timer;
    private TextMeshProUGUI timeText;

    public Battle_Time(BattleManager battleManager, TextMeshProUGUI timeText) : base(battleManager)
    {
        stageData = battleManager.currentStageData;
        timer = stageData.timeValue;
        this.timeText = timeText;
    }

    public void Update_Time()
    {
        if (stageData.timeType == TimeType.DisabledTime)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timeText.text = string.Format("{0}:{1:D2}", (int)timer / 60, (int)timer % 60);
            return;
        }

    }

    public void Set_SuddenDeath()
    {

    }
}
