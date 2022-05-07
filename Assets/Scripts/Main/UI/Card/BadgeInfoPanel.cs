using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;

namespace Main.Card
{
    public class BadgeInfoPanel : MonoBehaviour
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
        private PCInfoPanel _pcInfoPanel = null;

        private BadgeData _badgeData = null;

        /// <summary>
        /// ���� â�� �Է¹��� ���� �����Ϳ� �°� �����ϰ� ����
        /// </summary>
        /// <param name="skinData"></param>
        public void OnSetBadgePanel(BadgeData badgeData)
        {
            _badgeData = badgeData;
            _image.sprite = SkinData.GetSkin(badgeData._skinType);
            _nameText.text = badgeData._name;
            _decriptionText.text = badgeData._decription;
            gameObject.SetActive(true);
            SetEquipText();
        }

        /// <summary>
        /// ���� ���뿡 ������ �����Ѵ�
        /// </summary>
        public void OnSetBadge()
        {
            if (_pcInfoPanel.CheckAlreadyEquipBadge(_badgeData))
            {
                _pcInfoPanel.RemoveBadge(_badgeData);
            }
            else
            {
                _pcInfoPanel.AddBadge(_badgeData);
            }

            SetEquipText();
        }


        /// <summary>
        /// ���� â�� �ݴ´�
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
            if (_pcInfoPanel.CheckAlreadyEquipBadge(_badgeData))
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
