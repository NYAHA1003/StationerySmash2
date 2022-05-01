using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.Sticker
{


    public class LongSeeSticker : AbstractIdleSticker
    {
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }

        public override void RunIdleStickerAblity()
        {
            _myUnit.UnitStat.SetBonusRange(0.1f);
        }
    }
}