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
        /// ��ƼĿ â�� �Է¹��� ��ƼĿ �����Ϳ� �°� �����ϰ� ����
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
        /// ���� ī�忡 ��ƼĿ�� �����Ѵ�
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
        /// ��ƼĿ â�� �ݴ´�
        /// </summary>
        /// <param name="skinData"></param>
        public void OnCloseStickerPanel()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ���� �ؽ�Ʈ�� �����Ѵ�
        /// </summary>
        private void SetEquipText()
        {
            if (_cardInfoPanel.CheckAlreadyEquipSticker(_stickerData))
            {
                _equipText.text = "����";
            }
            else
			{
                _equipText.text = "����";
			}
        }

    }
}
