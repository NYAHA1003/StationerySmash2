using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class PencilNewSticker : AbstractSticker
{
    int ATKpercent = 50; // ���� hp ���ݷ� ��ȯ �ۼ�Ʈ
    int Weightpercent = 50;
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
        _matchState = eState.ATTACK;
    }

    public override void RunStickerAblity(eState eState)
    {
        if (_matchState != eState)
        {
            return;
        }
        _myUnit.UnitStat.IncreaseAttackPercent(_myUnit.UnitStat.LostHpPenrcent(ATKpercent));
        _myUnit.UnitStat.IncreaseWeightPercent(-1 * _myUnit.UnitStat.LostHpPenrcent(Weightpercent));
    }
}
