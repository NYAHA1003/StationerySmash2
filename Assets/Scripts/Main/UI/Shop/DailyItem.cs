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
    private Sprite _itemSprite; // 아이템 이미지
    [SerializeField]
    private TextMeshProUGUI _priceText; // 가격텍스트
    [SerializeField]
    private TextMeshProUGUI _itemNameText; // 이름 텍스트
    [SerializeField]
    private TextMeshProUGUI _countText; // 개수 텍스트

    public void SetCardInfo(DailyCardType dailyCardType, int itemCount)
    {
        DailyItemInfo dailyItemInfo = _dailyItemInfo.dailyItemInfos[(int)dailyCardType];
        _itemSprite = dailyItemInfo.itemSprite;
        _priceText.text = dailyItemInfo.card_price.ToString(); 
        _itemNameText.text = dailyItemInfo.card_Name;
        _countText.text = (dailyItemInfo.card_price * itemCount).ToString();

  
    }

}
