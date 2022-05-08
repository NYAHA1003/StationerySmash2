using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;


namespace Main.Card
{
    public class BadgeSetButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _badgeNameText = null;
        [SerializeField]
        private TextMeshProUGUI _badgeLevelText = null;
        [SerializeField]
        private Image _badgeImage = null;

        /// <summary>
        /// 뱃지 이미지 설정
        /// </summary>
        public void SetButtonImages(BadgeData badgeData)
        {
            _badgeNameText.text = badgeData._name;
            _badgeLevelText.text = $"Lv{badgeData._level}";
            _badgeImage.sprite = SkinData.GetSkin(badgeData._skinType);
        }
    }

}