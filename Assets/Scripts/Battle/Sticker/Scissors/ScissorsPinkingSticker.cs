using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle.Sticker
{


    public class ScissorsPinkingSticker : AbstractAttackSticker
    {
        int scratchStack = 0;
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }
        public override void RunAttackStickerAblity(ref AtkData atkData)
        {
            _myUnit.UnitStat.SubtractHP(_myUnit.UnitStat.MaxHp / 200 * 3 * scratchStack); // ���ľ���
        }
    }
}