using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Utill.Data
{
    public enum SkinThemeType
    {
        Normal = 0,

    }

    [System.Serializable]
    public class SkinMakeData
    {
        public Sprite sprite = null;
        public SkinType skinType = SkinType.SpriteNone;
        public string skinName = "";
        public List<MaterialData> _needMaterial = new List<MaterialData>();

    }
}