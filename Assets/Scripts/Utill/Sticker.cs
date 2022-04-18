using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    [System.Serializable]
    public class StickerData
    {
        public Sprite sprite;
        public StickerType stickerType;
        public int stickerLevel = 1;
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

    }
}
