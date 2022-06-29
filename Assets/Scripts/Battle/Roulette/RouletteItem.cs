using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public enum RoulletItemType
{
    Coin,
    Dalgona,
    Card
}

public class RouletteItem : MonoBehaviour
{
    [SerializeField]
    private Image _itemImage; // 현재는 사용 안함 
    [SerializeField]
    private TextMeshProUGUI _itemCountText;
    [SerializeField]
    private RouletteItemData rouletteItemData;

    public RoulletItemType RoulletItemType => rouletteItemData.rulletItemType; 
    public void SetUp(RouletteItemData rouletteItemData)
    {
        _itemImage.sprite = rouletteItemData._itemImage; 
        _itemCountText.text = rouletteItemData._itemCount.ToString();

    }
}
