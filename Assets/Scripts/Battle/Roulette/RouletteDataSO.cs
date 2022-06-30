using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/RouletteDataSO")]
public class RouletteDataSO : ScriptableObject
{
    public List<RouletteItemData> _rouletteItemDataList = new List<RouletteItemData>();
    public int[] dalgonaCounts;
    public int[] coinCounts;
    public Sprite[] itemSprites;
}
