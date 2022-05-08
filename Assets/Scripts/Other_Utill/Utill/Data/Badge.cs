using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    public enum BadgeType
    {
        None = 0,
        Health,
        Discount,
        Increase,
        TimeUp,
        TimeDown,
        Blanket,
        Thorn,
        Magnet_N,
        Magnet_S,
        GrowingSeed,
        Invincibel,
        Snack,
    }

    [System.Serializable]
    public class BadgeData
    {
        public int _level = 0;
        public string _name = "";
        public string _decription = "";
        public SkinType _skinType = SkinType.SpriteNone;
        public BadgeType _badgeType = BadgeType.None;

        /// <summary>
        /// 뱃지 데이터를 복사한다
        /// </summary>
        /// <returns></returns>
        public BadgeData DeepCopyBadgeData(BadgeSaveData badgeSaveData)
		{
            BadgeData badgeData = new BadgeData
            {
                _level = badgeSaveData._level,
                _badgeType = this._badgeType,
            };

            return badgeData;
		}

    }


    [System.Serializable]
    public class BadgeSaveData
    {
        public int _level;
        public BadgeType _BadgeType;
    }
}