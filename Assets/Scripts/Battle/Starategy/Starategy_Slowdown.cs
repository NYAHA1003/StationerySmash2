using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Starategy_Slowdown : AbstractStarategy
{
    public override void Run_Card(TeamType eTeam)
    {
        if(eTeam == TeamType.MyTeam)
        {
            Check_Range(_battleManager.CommandUnit._enemyUnitList, _card._dataBase.strategyData.starategyablityData);
        }
        else if (eTeam == TeamType.EnemyTeam)
        {
            Check_Range(_battleManager.CommandUnit._playerUnitList, _card._dataBase.strategyData.starategyablityData);
        }
    }

    private void Check_Range(List<Unit> list, params float[] value)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for(int i = 0; i < list.Count; i++)
        {
            if(Vector2.Distance(list[i].transform.position, mousePos) < value[3])
            {
                list[i].AddStatusEffect(Utill.AtkType.SlowDown, value);
            }
        }
    }
}
