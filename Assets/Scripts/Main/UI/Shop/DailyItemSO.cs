using UnityEngine;
using Utill.Data; 

[CreateAssetMenu(fileName = "DailyItem", menuName = "Scriptable Object/DailyItemDataSO")]
public class DailyItemSO : ScriptableObject
{
    public DailyCardType dailyCardType;
    public string card_Name;
    public string card_Description;
    public int card_price;
    public int count; 
}
