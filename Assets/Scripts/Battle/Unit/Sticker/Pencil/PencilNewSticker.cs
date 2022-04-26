using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class PencilNewSticker : AbstractSticker
{
    int ATKpercent = 50; // 잃은 hp 공격력 변환 퍼센트
    int Weightpercent = 50;
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
    }

    public override void RunStickerAblity()
    {
        _myUnit.UnitStat.IncreaseAttackPercent(_myUnit.UnitStat.LostHpPenrcent(ATKpercent));
        _myUnit.UnitStat.IncreaseWeightPercent(-1 * _myUnit.UnitStat.LostHpPenrcent(Weightpercent));
    }
}
