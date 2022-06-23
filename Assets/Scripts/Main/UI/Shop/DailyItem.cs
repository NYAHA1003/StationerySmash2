using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
public class DailyItem : MonoBehaviour
{

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

    private DailyItemInfo _dailyItemInfo;
    public DailyItemInfo DailyItemInfo => _dailyItemInfo;
    private IPerchase _dailyItem;
    [SerializeField]
    private Button _itemButton;
    private bool _isbuy  = false;
    public void SetCardInfo(DailyItemInfo dailyItemInfo,int itemCount = 0)
    {
        _dailyItemInfo = dailyItemInfo; 
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
        _countText.text = string.Format("X {0}",itemCount.ToString());
        bool isbuy = false;
        _itemButton.onClick.AddListener(() => dailyItemInfo._dailyItem.Purchase(out isbuy)); 
        _itemButton.onClick.AddListener(() => Purchased(isbuy));

    }

    /// <summary>
    /// ���ŵ�
    /// </summary>
   public void Purchased(bool isbuy)
    {
        _isbuy = isbuy; 
        if (_isbuy)
		{
            Purchase(); 
            _dailyItemInfo._isBuy = true; 

        }
    }

    /// <summary>
    /// ���Ž� �ٲ�� �κ� 
    /// </summary>
    public void Purchase()
    {
        _itemButton.enabled = false;
        _blackImage.SetActive(true);
    }
}
