using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Main.Skin
{
    [System.Serializable]
    public class SkinMakeData
    {
        public Sprite sprite = null;
        public string skinName = "";
        public List<MaterialData> _needMaterial = new List<MaterialData>();

    }
}