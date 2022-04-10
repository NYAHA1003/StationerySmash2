using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperSticker : AbstractSticker
{
    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.SetBonusMaxHP(100);
    }
}
