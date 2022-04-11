using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum BadgeType
    {
        None = 0,
        Health,
        Discount,
        Increase,
        TimeUp,
        TimeDown,
    }

    [System.Serializable]
    public class BadgeData
    {
        public BadgeType _badgeType;
        public int _level;
    }
}