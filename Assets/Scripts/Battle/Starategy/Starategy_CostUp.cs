using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Starategy_CostUp : AbstractStarategy
{
    public override void Run_Card(TeamType eTeam)
    {
        _battleManager.CommandCost.AddCost(1 * _card.Grade);
    }
}
