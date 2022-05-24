using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using TMPro;

namespace Main.Card
{
    public class StickerInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private Image _image = null;
        [SerializeField]
        private TextMeshProUGUI _nameText = null;
        [SerializeField]
        private TextMeshProUGUI _decriptionText = null;
        [SerializeField]
        private TextMeshProUGUI _equipText = null;
        [SerializeField]
        private CardInfoPanel _cardInfoPanel = null;

        private StickerData _stickerData = null;

        /// <summary>
        /// 스티커 창을 입력받은 스티커 데이터에 맞게 설정하고 연다
        /// </summary>
        /// <param name="skinData"></param>
        public void OnSetSkickerPanel(StickerData stickerData)
        {
            _stickerData = stickerData;
            _image.sprite = SkinData.GetSkin(stickerData.SkinType);
            _nameText.text = stickerData.Name;
            _decriptionText.text = stickerData.Description;
            gameObject.SetActive(true);
            SetEquipText();
        }

        /// <summary>
        /// 현재 카드에 스티커를 적용한다
        /// </summary>
        public void OnSetSticker()
        {
            if (_cardInfoPanel.CheckAlreadyEquipSticker(_stickerData))
			{
                _cardInfoPanel.ReleaseSticker();
			}
            else
			{
                _cardInfoPanel.SetSticker(_stickerData);
			}

            SetEquipText();
        }


        /// <summary>
        /// 스티커 창을 닫는다
        /// </summary>
        /// <param name="skinData"></param>
        public void OnCloseStickerPanel()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 장착 텍스트를 설정한다
        /// </summary>
        private void SetEquipText()
        {
            if (_cardInfoPanel.CheckAlreadyEquipSticker(_stickerData))
            {
                _equipText.text = "해제";
            }
            else
			{
                _equipText.text = "장착";
			}
        }

    }
}
