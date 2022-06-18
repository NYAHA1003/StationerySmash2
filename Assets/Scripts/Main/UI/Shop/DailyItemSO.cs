using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using UnityEngine.UI; 

namespace Utill.Data
{
    // ī�� ��Ů
    public enum Grade
    {
        Null, // ��� ����
        Common, //�Ϲ�
        Rare, //����
        Epic //���� 
    }
}
[System.Serializable]
public class DailyItemInfo
{
    //public DailyCardType dailyCardType;
    public Grade _grade;
    public Sprite _itemSprite;
    public string _cardName;
    public int _cardPrice;
    public int _itemCount;
    public CardNamingType _unitCardType; 
    public IPerchase _dailyItem; 

    public void DeepCopy(DailyItemInfo dailyItemInfo)
    {
        this._grade = dailyItemInfo._grade;
        this._itemSprite = dailyItemInfo._itemSprite;
        this._cardName = dailyItemInfo._cardName;
        this._cardPrice = dailyItemInfo._cardPrice;
        this._itemCount = dailyItemInfo._itemCount; 
    }
}

[CreateAssetMenu(fileName = "DailyItem", menuName = "Scriptable Object/DailyItemDataSO")]
public class DailyItemSO : ScriptableObject
{
    [Header("���ϻ��� �Ѿ����� ����")]
    public List<DailyItemInfo> dailyItemInfos = new List<DailyItemInfo>(); 
}
