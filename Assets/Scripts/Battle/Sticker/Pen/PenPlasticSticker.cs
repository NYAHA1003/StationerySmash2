using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.Sticker
{


    public class PenPlasticSticker : AbstractDamagedSticker
    {
        private int plusHp = 15;
        private int damageDecrese = -10;
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }
        public override void RunDamagedStickerAblity(ref AtkData atkData)
        {
            _myUnit.UnitStat.SetBonusMaxHPPercent(plusHp);
            _myUnit.UnitStat.IncreaseDamagedPercent(damageDecrese);
        }
    }
}