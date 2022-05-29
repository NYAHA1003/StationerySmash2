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
        [SerializeField]
        private RectTransform _stickerRect;

        public CardNamingType _cardNamingType { get; private set; } = CardNamingType.None;

        /// <summary>
        /// ī�� ����, �����ڸ��� �������
        /// </summary>
        /// <param name="cardData"></param>
        public void SetCard(CardData cardData)
        {
            _cardNamingType = cardData._cardNamingType;
            _cardImage.sprite = SkinData.GetSkin(cardData._skinData._skinType);
            _unitNameText.text = cardData.card_Name;
            _CostText.text = $"{cardData.card_Cost}";
            _stickerRect.gameObject.SetActive(false);

            if(StickerData.CheckCanSticker(cardData))
            {
                UnitData unitData = UnitDataManagerSO.FindUnitData(cardData.unitType);
                StickerData stickerData = StickerDataManagerSO.FindStickerData(unitData._stickerType);
                _stickerImage.sprite = SkinData.GetSkin(stickerData.SkinType);
                _stickerRect.anchoredPosition = StickerData.ReturnStickerPos(cardData.unitType);
                _stickerRect.gameObject.SetActive(true);
            }
            else
            {
                _stickerRect.gameObject.SetActive(false);
            }
        }
    }
}