using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum CollectionType
    {
        None = 0,
        Normal,
        Skin,

    }

    public enum CollectionThemeType
    {
        None = 0,
    }

    [System.Serializable]
    public class CollectionData
    {
        public string _name = "";
        public Sprite _collectionSprite = null;
        public CollectionType _collectionType = CollectionType.None;
        public CollectionThemeType _collectionThemeType = CollectionThemeType.None;
        public List<CardNamingType> _needCardNamingType = null;
    }
}
