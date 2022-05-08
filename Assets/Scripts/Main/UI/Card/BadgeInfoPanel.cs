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
        /// 뱃지 창을 입력받은 뱃지 데이터에 맞게 설정하고 연다
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
        /// 현재 필통에 뱃지를 적용한다
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
        /// 뱃지 창을 닫는다
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
            if (_pcInfoPanel.CheckAlreadyEquipBadge(_badgeData))
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
