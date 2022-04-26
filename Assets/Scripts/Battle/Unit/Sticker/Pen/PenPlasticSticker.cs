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
    }
    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.SetBonusMaxHPPercent(plusHp);
        _myUnit.UnitStat.IncreaseDamagedPercent(damageDecrese);
    }
}
