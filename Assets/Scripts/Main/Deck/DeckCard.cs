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
        [SerializeField, Header("유닛용")]
        private Image _stickerImage;
        [SerializeField]
        private RectTransform _stickerRect;

        public CardData _cardData { get; private set; }

        /// <summary>
        /// 카드 설정, 만들자마자 해줘야함
        /// </summary>
        /// <param name="cardData"></param>
        public void SetCard(CardData cardData)
        {
            _cardData = cardData;
            _cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
            _unitNameText.text = cardData.card_Name;
            _CostText.text = $"{cardData.card_Cost}";
            _stickerRect.gameObject.SetActive(false);

            if(StickerData.CheckCanSticker(cardData))
            {
                _stickerImage.sprite = SkinData.GetSkin(cardData.unitData.stickerData._skinType);
                _stickerRect.anchoredPosition = StickerData.ReturnStickerPos(cardData.unitData.unitType);
                _stickerRect.gameObject.SetActive(true);
            }
            else
            {
                _stickerRect.gameObject.SetActive(false);
            }
        }
    }
}