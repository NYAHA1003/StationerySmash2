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
    private bool isSuddenDeath;
    private bool isFinallyEnd;

    public Battle_Time(BattleManager battleManager, TextMeshProUGUI timeText) : base(battleManager)
    {
        stageData = battleManager.currentStageData;
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
        battleManager.battle_Card.Clear_Cards();
        battleManager.battle_Unit.Clear_Unit();

        if (!isSuddenDeath)
        {
            battleManager.battle_Card.Set_MaxCard(8);
            battleManager.battle_Cost.Set_CostSpeed(500);
            isSuddenDeath = true;
            timer = 60;

            Debug.Log("���絥�� ����");
            return;
        }

        //ü�� ��
        if(battleManager.unit_MyDatasTemp[0].hp > battleManager.unit_EnemyDatasTemp[0].hp)
        {
            Debug.Log("���絥�� �¸�");
            isFinallyEnd = true;
            return;
        }
        if (battleManager.unit_MyDatasTemp[0].hp < battleManager.unit_EnemyDatasTemp[0].hp)
        {
            Debug.Log("���絥�� �й�");
            isFinallyEnd = true;
            return;
        }

        Debug.Log("���絥�� ���º�");
        isFinallyEnd = true;
    }
}
