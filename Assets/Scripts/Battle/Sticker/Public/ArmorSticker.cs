using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.Sticker
{


    public class ArmorSticker : AbstractIdleSticker
    {
        public override void RunIdleStickerAblity()
        {
            _myUnit.UnitStat.DecreseDamage(5);
        }
    }

}
