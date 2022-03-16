using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStarategy
{
    public abstract void Run_Card(BattleManager battleManager, int grade, params float[] value);
}
