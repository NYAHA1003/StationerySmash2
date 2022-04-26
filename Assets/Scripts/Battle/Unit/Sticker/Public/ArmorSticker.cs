using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class ArmorSticker : AbstractSticker
{
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
    }

    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.DecreseDamage(5);
    }
}
