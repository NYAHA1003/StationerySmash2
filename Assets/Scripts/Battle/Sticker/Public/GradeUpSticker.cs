using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class GradeUpSticker : AbstractIdleSticker
{
    int levelUp = 1;
    public override void RunIdleStickerAblity()
    {
        _myUnit.UnitStat.GradeUp(levelUp);

    }
}
