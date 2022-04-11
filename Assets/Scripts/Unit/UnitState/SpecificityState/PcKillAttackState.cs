using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PcKillAttackState : AbstractAttackState
{
    protected override void SetAttackData(ref AtkData atkData)
    {
        base.SetAttackData(ref atkData);
        if (_targetUnit.UnitData.unitType == UnitType.PencilCase)
        {
            atkData.Reset_Damage(atkData.damage * 2);
        }
        else
        {
            atkData.Reset_Damage(atkData.damage / 2);
        }
    }
}
