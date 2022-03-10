using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStarategy
{
    public abstract void Run_Card(BattleManager battleManager);
}
