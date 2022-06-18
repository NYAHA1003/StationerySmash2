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
    private Image _itemImage; // ������ �̹���
    [SerializeField]
    private GameObject _blackImage; // ���Ž� ���� �̹��� 
    [SerializeField]
    private TextMeshProUGUI _priceText; // �����ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI _itemNameText; // �̸� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI _countText; // ���� �ؽ�Ʈ

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
            _priceText.text = "����";
        }
        else
        {
            _priceText.text = string.Format("{0} ��",(dailyItemInfo._cardPrice * itemCount).ToString());
        }
        _itemNameText.text = dailyItemInfo._cardName;
        _countText.text = string.Format("X{0}",itemCount.ToString());

        _itemButton.onClick.AddListener(() => dailyItemInfo._dailyItem.Purchase()); 
        _itemButton.onClick.AddListener(() => Purchased());

    }

    /// <summary>
    /// ���ŵ�
    /// </summary>
   public void Purchased()
    {
        _itemButton.enabled = false;
        _blackImage.SetActive(true);
    }

}
