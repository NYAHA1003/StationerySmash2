using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAbility : AbstractPencilCaseAbility
{
    public override void RunPencilCaseAbility()
    {
        _battleManager.StartCoroutine(BuffDeBuff());
    }

    private IEnumerator BuffDeBuff()
    {
        for (int i = 0; i < _battleManager.CommandUnit._playerUnitList.Count; i++)
        {
            Unit unit = _battleManager.CommandUnit._playerUnitList[i];

            unit.AddStatusEffect(Utill.AtkType.Rage, 1, 100, 100);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _battleManager.CommandUnit._playerUnitList.Count; i++)
        {
            Unit unit = _battleManager.CommandUnit._playerUnitList[i];

            unit.AddStatusEffect(Utill.AtkType.Rage, 1, -100, -100);
        }
    }
}
