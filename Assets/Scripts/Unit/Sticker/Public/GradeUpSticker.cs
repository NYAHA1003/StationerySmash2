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
        _matchState = eState.IDLE;
    }
    public override void RunStickerAblity(eState eState)
    {
        if (_matchState != eState)
        {
            return;
        }
        _myUnit.UnitStat.GradeUp(levelUp);

    }
}
