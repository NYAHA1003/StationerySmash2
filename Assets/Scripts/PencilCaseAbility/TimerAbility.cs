using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAbility : AbstractPencilCaseAbility
{
    public override void RunPencilCaseAbility()
    {
        for (int i = 0; i < _battleManager.CommandUnit._playerUnitList.Count; i++)
        {
            Unit unit = _battleManager.CommandUnit._playerUnitList[i];

            unit.AddStatusEffect(Utill.AtkType.Exch, 1, 100, 100);
        }
    }
}
