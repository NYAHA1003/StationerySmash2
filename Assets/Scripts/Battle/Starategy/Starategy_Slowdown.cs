using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Starategy_Slowdown : IStarategy
{
    public void Run_Card(BattleManager battleManager, TeamType eTeam ,int grade, params float[] value)
    {
        if(eTeam == TeamType.MyTeam)
        {
            Check_Range(battleManager.unit_EnemyDatasTemp, value);
        }
        else if (eTeam == TeamType.EnemyTeam)
        {
            Check_Range(battleManager.unit_MyDatasTemp, value);
        }
    }

    private void Check_Range(List<Unit> list, params float[] value)
    {
        for(int i = 0; i < list.Count; i++)
        {
            list[i].Add_StatusEffect(Utill.AtkType.SlowDown, value);
        }
    }
}
