using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public abstract class AbstractStarategy
{
    protected BattleManager _battleManager;
    protected CardMove _card;


    public void SetBattleManager(BattleManager battleManager)
    {
        this._battleManager = battleManager;
    }

    public void SetCard(CardMove card)
    {
        this._card = card;
    }

    public abstract void Run_Card(TeamType eTeam);
}
