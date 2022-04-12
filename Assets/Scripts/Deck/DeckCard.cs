using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckCard : MonoBehaviour
{
    [SerializeField]
    private Image _unitImage;
    [SerializeField]
    private Image _stickerImage;
    [SerializeField]
    private TextMeshProUGUI _unitNameText;
    [SerializeField]
    private TextMeshProUGUI _CostText;

    public void SetCard(CardData cardData)
    {

    }
}
