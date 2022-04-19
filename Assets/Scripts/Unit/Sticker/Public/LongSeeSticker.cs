using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class LongSeeSticker : AbstractSticker
{
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
        _myUnit.UnitStat.SetBonusRange(1);
    }
}
