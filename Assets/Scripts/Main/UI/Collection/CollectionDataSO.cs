using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Main.Collection
{
    [CreateAssetMenu(fileName = "CollectionDataSO ", menuName = "Scriptable Object/CollectionDataSO")]
    public class CollectionDataSO : ScriptableObject
    {
        public List<CollectionData> _collectionDatas = new List<CollectionData>();
    }
}