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
    private Image _itemImage; // 아이템 이미지
    [SerializeField]
    private TextMeshProUGUI _priceText; // 가격텍스트
    [SerializeField]
    private TextMeshProUGUI _itemNameText; // 이름 텍스트
    [SerializeField]
    private TextMeshProUGUI _countText; // 개수 텍스트

    public void SetCardInfo(DailyItemInfo dailyItemInfo, int itemCount = 0)
    {
        Transform itemInfoObj = transform.Find("ItemInfo");
        _itemImage = transform.Find("ItemImage").GetComponent<Image>();
        _priceText = itemInfoObj.Find("PriceText").GetComponent<TextMeshProUGUI>();
        _itemNameText = itemInfoObj.Find("NameText").GetComponent<TextMeshProUGUI>();
        _countText = itemInfoObj.Find("CountText").GetComponent<TextMeshProUGUI>();

        // DailyItemInfo dailyItemInfo = _dailyItemInfo.dailyItemInfos[(int)dailyCardType];
        _itemImage.sprite = dailyItemInfo._itemSprite;
        itemCount = dailyItemInfo._itemCount;
        if (dailyItemInfo._cardPrice == 0)
        {
            _priceText.text = "무료";
        }
        else
        {
            _priceText.text = (dailyItemInfo._cardPrice * itemCount).ToString();
        }
        _itemNameText.text = dailyItemInfo._cardName;
        _countText.text = itemCount.ToString();


    }

}
