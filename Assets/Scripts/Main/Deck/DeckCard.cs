using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;

namespace Main.Deck
{
    public class DeckCard : MonoBehaviour
    {
        [SerializeField]
        private Image _cardImage;
        [SerializeField]
        private TextMeshProUGUI _unitNameText;
        [SerializeField]
        private TextMeshProUGUI _CostText;
        [SerializeField, Header("���ֿ�")]
        private Image _stickerImage;

        public CardData _cardData { get; private set; }

        /// <summary>
        /// ī�� ����, �����ڸ��� �������
        /// </summary>
        /// <param name="cardData"></param>
        public void SetCard(CardData cardData)
        {
            _cardData = cardData;
            //_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
            _unitNameText.text = cardData.card_Name;
            _CostText.text = $"{cardData.card_Cost}";

            if (cardData.cardType == CardType.SummonUnit)
            {
                if (cardData.unitData?.stickerData != null)
                {
                    _stickerImage.sprite = cardData.unitData.stickerData._sprite;
                }
            }
        }
    }
}