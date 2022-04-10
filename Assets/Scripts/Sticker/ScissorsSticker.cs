using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsSticker : AbstractSticker
{
    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.SetBonusAttack(20);
    }
}
