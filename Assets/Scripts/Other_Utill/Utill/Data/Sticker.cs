using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    [System.Serializable]
    public class StickerData
    {
        public Sprite _sprite;
        public string _name;
        public string _decription;
        public UnitType _onlyUnitType;
        public StickerType _stickerType;
        public int _stickerLevel = 1;
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
