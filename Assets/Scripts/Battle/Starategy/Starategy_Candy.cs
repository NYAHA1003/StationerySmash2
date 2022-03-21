using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Starategy_Candy : IStarategy
{
    public void Run_Card(BattleManager battleManager, TeamType eTeam, int grade, params float[] value)
    {
        if(eTeam == TeamType.MyTeam)
        {
            Check_Range(battleManager, battleManager.unit_EnemyDatasTemp, true, value);
        }
        else if (eTeam == TeamType.EnemyTeam)
        {
            Check_Range(battleManager, battleManager.unit_MyDatasTemp, false, value);
        }
    }
    private void Check_Range(BattleManager battleManager, List<Unit> list, bool isMyTeam , params float[] value)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < list.Count; i++)
        {
            if(isMyTeam)
            {
                if (list[i].transform.position.x < 0)
                {
                    list[i].Run_Damaged(new AtkData(null, 10, 20, 0, 45, !isMyTeam, 90000 + i));
                }
            }
            else if (list[i].transform.position.x > 0)
            {
                list[i].Run_Damaged(new AtkData(null, 10, 20, 0, 45, !isMyTeam, 90000 + i));
            }
        }
    }
}
