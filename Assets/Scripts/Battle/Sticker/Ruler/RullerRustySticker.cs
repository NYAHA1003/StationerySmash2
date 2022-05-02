using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.Sticker
{


    public class RullerRustySticker : AbstractIdleSticker
    {
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }
        public override void RunIdleStickerAblity()
        {
            _myUnit.UnitStat.IncreaseWeightPercent(100);
        }
    }
}