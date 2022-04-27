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
        atkData.Reset_Type(EffAttackType.PCKill);
    }
}
