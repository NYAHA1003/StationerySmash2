using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
namespace Battle.Starategy
{


    public class Starategy_Rage : AbstractStarategy
    {
        public override void Run_Card(TeamType eTeam)
        {
            _battleManager.CostComponent.AddCost(3);
        }
    }
}