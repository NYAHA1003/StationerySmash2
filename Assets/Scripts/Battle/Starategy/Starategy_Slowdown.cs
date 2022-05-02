using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utill.Data;
using Utill.Tool;

namespace Battle.Starategy
{


    public class Starategy_Slowdown : AbstractStarategy
    {
        public override void Run_Card(TeamType eTeam)
        {
            if (eTeam == TeamType.MyTeam)
            {
                Check_Range(_battleManager.CommandUnit._enemyUnitList, _card.DataBase.strategyData.starategyablityData);
            }
            else if (eTeam == TeamType.EnemyTeam)
            {
                Check_Range(_battleManager.CommandUnit._playerUnitList, _card.DataBase.strategyData.starategyablityData);
            }
        }

        private void Check_Range(List<Unit> list, params float[] value)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < list.Count; i++)
            {
                //value[3] = ¹üÀ§
                try
                {
                    if (Vector2.Distance(list[i].transform.position, mousePos) < value[3])
                    {
                        list[i].AddStatusEffect(EffAttackType.SlowDown, value);
                    }
                }
                catch
                {
                    //int a = 0;
                }
            }
        }
    }
}