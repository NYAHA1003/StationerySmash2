using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class ScissorsPinkingSticker: AbstractSticker
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
        _myUnit.UnitStat.SubtractHP(_myUnit.UnitStat.MaxHp / 200 * 3); //초마다로 바꿔야함
    }
}
