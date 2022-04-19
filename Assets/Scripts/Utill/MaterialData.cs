using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum MaterialType
    {
        None = 0,
        ClothPiece,
        PlasticPiece,
        GlassPiece,
        WaterDrop,
        Sticky,
        Sparkling,
        Snowball,
        Sunlight,
        Seam,
        Sugar,
    }

    [System.Serializable]
    public class MaterialData
    {
        public Sprite _sprite;
        public MaterialType _materialType;
        public string name;
        public int _count;
    }

}
