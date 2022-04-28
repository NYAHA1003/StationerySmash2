using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.Starategy
{


    public class Starategy_Rage : AbstractStarategy
    {
        public override void Run_Card(TeamType eTeam)
        {
            _battleManager.CommandCost.AddCost(3);
        }
    }
}