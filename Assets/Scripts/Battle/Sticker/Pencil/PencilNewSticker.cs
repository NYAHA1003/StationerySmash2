using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle.Sticker
{


    public class PencilNewSticker : AbstractDamagedSticker
    {
        int ATKpercent = 50; // ���� hp ���ݷ� ��ȯ �ۼ�Ʈ
        int Weightpercent = 50;
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }

        public override void RunDamagedStickerAblity(ref AtkData atkData)
        {
            _myUnit.UnitStat.IncreaseAttackPercent(_myUnit.UnitStat.LostHpPenrcent(ATKpercent));
            _myUnit.UnitStat.IncreaseWeightPercent(-1 * _myUnit.UnitStat.LostHpPenrcent(Weightpercent));
        }
    }
}