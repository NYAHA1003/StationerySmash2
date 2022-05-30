using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;

namespace Main.Card
{
	public class StickerChangeButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _stickerNameText = null;
        [SerializeField]
        private TextMeshProUGUI _stickerLevelText = null;
        [SerializeField]
        private Image _stickerImage = null;

        /// <summary>
        /// 스킨 이미지 설정
        /// </summary>
        /// <param name="skinData"></param>
        public void SetButtonImages(StickerData stickerData)
        {
            _stickerNameText.text = stickerData._name;
            _stickerLevelText.text = $"Lv{stickerData._level}";
            _stickerImage.sprite = SkinData.GetSkin(stickerData._skinType);
        }
    }
}
