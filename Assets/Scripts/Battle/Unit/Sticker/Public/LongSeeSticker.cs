using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class LongSeeSticker : AbstractSticker
{
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
    }

    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.SetBonusRange(1);
    }
}
