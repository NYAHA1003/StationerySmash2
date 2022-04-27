using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class RockSticker : AbstractIdleSticker
{
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
    }
    public override void RunIdleStickerAblity()
    {
        _myUnit.UnitStat.SetBonusMoveSpeed(1);
    }
}
