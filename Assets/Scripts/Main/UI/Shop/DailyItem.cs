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

    public void SetCardInfo(DailyCardType dailyCardType)
    {
        DailyItemInfo dailyItemInfo = _dailyItemInfo.dailyItemInfos[(int)dailyCardType];
        _itemSprite = dailyItemInfo.itemSprite;
        _priceText.text = dailyItemInfo.card_price.ToString(); 
        _itemNameText.text = dailyItemInfo.card_Name;
        _countText.text = dailyItemInfo.count.ToString();

        switch (dailyCardType)
        {
            case DailyCardType.Pencil:
                break;
            case DailyCardType.Sharp:
                break;
            case DailyCardType.Eraser:
                break;
            case DailyCardType.Scissors:
                break;
            case DailyCardType.Glue:
                break;
            case DailyCardType.Ruler:
                break;
            case DailyCardType.Boxcutter:
                break;
            case DailyCardType.Postit:
                break;
            case DailyCardType.Sharplead:
                break;
            case DailyCardType.ballpointPen:
                break;
            case DailyCardType.EraserPiece:
                break;
            case DailyCardType.PostitSheet:
                break;
        }
    }

}
