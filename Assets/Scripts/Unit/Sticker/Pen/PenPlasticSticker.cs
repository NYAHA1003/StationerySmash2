using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class PenPlasticSticker : AbstractSticker
{
    private int plusHp = 15;
    private int damageDecrese = -10;
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
        _myUnit.UnitStat.SetBonusMaxHPPercent(plusHp);
        _myUnit.UnitStat.IncreaseDamagedPercent(damageDecrese);
    }
}
