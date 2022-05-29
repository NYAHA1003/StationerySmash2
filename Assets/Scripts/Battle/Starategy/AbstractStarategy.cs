using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle.Starategy
{


    public abstract class AbstractStarategy
    {
        protected BattleManager _battleManager;
        protected CardObj _card;
        protected StrategyData _starategyData;
        public void SetBattleManager(BattleManager battleManager)
        {
            this._battleManager = battleManager;
        }

        public void SetCard(CardObj card)
        {
            this._card = card;
        }

        public void SetValuse(params float[] valuse)
        {
            _starategyData._starategyablityData = valuse;
        }
        public abstract void Run_Card(TeamType eTeam);
    }
}