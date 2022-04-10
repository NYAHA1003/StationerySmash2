using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSticker : AbstractSticker
{
    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.SetBonusMoveSpeed(1);
    }
}
