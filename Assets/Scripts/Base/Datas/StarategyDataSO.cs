using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StarategyDataSO", menuName = "Scriptable Object/StarategyDataSO")]
public class StarategyDataSO : ScriptableObject
{
    public List<CardData> starategyDatas = new List<CardData>();
}
