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
        public BadgeType _badgeType;
        public int _level;
    }
}