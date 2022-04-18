using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum MaterialType
    {
        None = 0,
        
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
