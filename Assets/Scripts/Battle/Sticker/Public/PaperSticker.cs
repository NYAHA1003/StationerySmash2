using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.Sticker
{


    public class PaperSticker : AbstractIdleSticker
    {
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }

        public override void RunIdleStickerAblity()
        {
            _myUnit.UnitStat.SetBonusMaxHP(100);
        }
    }
}