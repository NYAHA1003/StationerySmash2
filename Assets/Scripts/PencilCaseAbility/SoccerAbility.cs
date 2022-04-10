using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class SoccerAbility : AbstractPencilCaseAbility
{
    float speed = 10;
    float duration = 4;
    public override void RunPencilCaseAbility()
    {
        _battleManager.CommandCost.AddCostSpeed(speed);
        Wait();
        _battleManager.CommandCost.AddCostSpeed(-speed);
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(duration);
    }
}
