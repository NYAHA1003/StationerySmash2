using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class ScissorsPinkingSticker: AbstractSticker
{
    int scratchStack = 0;
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
        _myUnit.UnitStat.SubtractHP(_myUnit.UnitStat.MaxHp / 200 * 3 * scratchStack); // 고쳐야함
    }
}
