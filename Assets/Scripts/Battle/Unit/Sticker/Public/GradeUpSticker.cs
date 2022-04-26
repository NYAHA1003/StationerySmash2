using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class GradeUpSticker : AbstractSticker
{
    int levelUp = 1;
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
    }
    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.GradeUp(levelUp);

    }
}
