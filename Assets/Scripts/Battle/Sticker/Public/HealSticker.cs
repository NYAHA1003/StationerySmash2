using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.Sticker
{


    public class HealSticker : AbstractIdleSticker
    {
        public override void SetSticker(Unit unit)
        {
            base.SetSticker(unit);
        }

        public override void RunIdleStickerAblity()
        {
            //�ڷ�ƾ���� ���� ����
            _myUnit.UnitStat.SubtractHP(-3);
        }
    }
}