using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Starategy_CostUp : IStarategy
{
    public void Run_Card(BattleManager battleManager, TeamType eTeam, int grade, params float[] value)
    {
        battleManager.battle_Cost.Add_Cost(1 * grade);
    }
}
