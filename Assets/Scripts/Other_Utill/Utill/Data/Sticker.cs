using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    [System.Serializable]
    public class StickerData
    {
        public SkinType _skinType;
        public string _name;
        public string _decription;
        public UnitType _onlyUnitType;
        public StickerType _stickerType;
        public int _stickerLevel = 1;


        /// <summary>
        /// ���� Ÿ�Կ� ���� ��ƼĿ ��ġ ��ȯ
        /// </summary>
        public static Vector2 ReturnStickerPos(UnitType unitType)
		{
            switch(unitType)
			{
                case UnitType.None:
                    return Vector2.zero;
			}
            return Vector2.zero;
		}


        /// <summary>
        /// ��ƼĿ�� ����� �� �ִ��� üũ
        /// </summary>
        /// <returns></returns>
        public static bool CheckCanSticker(CardData cardData)
        {
            if (cardData.cardType == CardType.SummonUnit)
            {
                if (cardData.unitData?.stickerData._stickerType != StickerType.None)
                {
                    return true;
                }
            }
            return false;
        }

    }
    public enum StickerType
    {
        None = 0,
        Rock = 1,
        Paper,
        Scissors,
        Eraser,
        Armor,
        Run,
        Heal,
        LongSee,
        Heavy,
        Invincible,
        PencilNew,
        ScissorPinking,
        RustyRuller,
        PlasticPen,
    }
}
