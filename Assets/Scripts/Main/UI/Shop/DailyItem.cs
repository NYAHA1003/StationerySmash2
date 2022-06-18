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
    private GameObject _blackImage; // 구매시 가릴 이미지 
    [SerializeField]
    private TextMeshProUGUI _priceText; // 가격텍스트
    [SerializeField]
    private TextMeshProUGUI _itemNameText; // 이름 텍스트
    [SerializeField]
    private TextMeshProUGUI _countText; // 개수 텍스트

    private IPerchase _dailyItem;
    [SerializeField]
    private Button _itemButton; 
    public void SetCardInfo(DailyItemInfo dailyItemInfo,int itemCount = 0)
    {
        // DailyItemInfo dailyItemInfo = _dailyItemInfo.dailyItemInfos[(int)dailyCardType];
        _itemImage.sprite = dailyItemInfo._itemSprite;
        itemCount = dailyItemInfo._itemCount;
        if (dailyItemInfo._cardPrice == 0)
        {
            _priceText.text = "무료";
        }
        else
        {
            _priceText.text = string.Format("{0} 원",(dailyItemInfo._cardPrice * itemCount).ToString());
        }
        _itemNameText.text = dailyItemInfo._cardName;
        _countText.text = string.Format("X{0}",itemCount.ToString());

        _itemButton.onClick.AddListener(() => dailyItemInfo._dailyItem.Purchase()); 
        _itemButton.onClick.AddListener(() => Purchased());

    }

    /// <summary>
    /// 구매됨
    /// </summary>
   public void Purchased()
    {
        _itemButton.enabled = false;
        _blackImage.SetActive(true);
    }

}
