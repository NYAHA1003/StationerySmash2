using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
public class DailyItem : MonoBehaviour
{
    [SerializeField]
    private DailyItemSO _dailyItemInfo; 

    [SerializeField]
    private Sprite _itemSprite; // ������ �̹���
    [SerializeField]
    private TextMeshProUGUI _priceText; // �����ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI _itemNameText; // �̸� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI _countText; // ���� �ؽ�Ʈ

    public void SetCardInfo(DailyCardType dailyCardType, int itemCount)
    {
        DailyItemInfo dailyItemInfo = _dailyItemInfo.dailyItemInfos[(int)dailyCardType];
        _itemSprite = dailyItemInfo.itemSprite;
        _priceText.text = dailyItemInfo.card_price.ToString(); 
        _itemNameText.text = dailyItemInfo.card_Name;
        _countText.text = (dailyItemInfo.card_price * itemCount).ToString();

  
    }

}
