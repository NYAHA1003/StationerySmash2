using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckCard : MonoBehaviour
{
    [SerializeField]
    private Image _cardImage;
    [SerializeField]
    private TextMeshProUGUI _unitNameText;
    [SerializeField]
    private TextMeshProUGUI _CostText;
    [SerializeField, Header("유닛용")]
    private Image _stickerImage;

    /// <summary>
    /// 카드 설정, 만들자마자 해줘야함
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCard(CardData cardData)
    {
        _cardImage.sprite = cardData.card_Sprite;
        _unitNameText.text = cardData.card_Name;
        _CostText.text = $"{cardData.card_Cost}";

        if(cardData.cardType == Utill.CardType.SummonUnit)
        {
            if(cardData.unitData?.stickerData != null)
            {
                _stickerImage.sprite = cardData.unitData.stickerData.sprite;
            }
        }
    }
}
