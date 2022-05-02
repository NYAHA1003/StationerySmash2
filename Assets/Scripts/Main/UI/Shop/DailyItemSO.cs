using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using UnityEngine.UI; 
[System.Serializable]
public class DailyItemInfo
{
    public DailyCardType dailyCardType;
    public Sprite itemSprite; 
    public string card_Name;
    public int card_price;
}

[CreateAssetMenu(fileName = "DailyItem", menuName = "Scriptable Object/DailyItemDataSO")]
public class DailyItemSO : ScriptableObject
{
    [Header("일일상점 총아이템 정보")]
    public List<DailyItemInfo> dailyItemInfos = new List<DailyItemInfo>(); 
}
