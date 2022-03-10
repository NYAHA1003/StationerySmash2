using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starategy_State : IStarategy
{
    public void Run_Card(BattleManager battleManager)
    {
        battleManager.battle_Cost.Add_Cost(3);
    }
}
