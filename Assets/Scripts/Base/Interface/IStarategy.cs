using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public interface IStarategy
{
    public abstract void Run_Card(BattleManager battleManager, TeamType eTeam , int grade, params float[] value);
}
