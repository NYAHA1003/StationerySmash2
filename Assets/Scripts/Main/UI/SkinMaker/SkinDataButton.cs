using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;

namespace Main.Skin
{
    /// <summary>
    /// 스킨 데이터 설정 버튼 클래스
    /// </summary>
    public class SkinDataButton : MonoBehaviour
    {
        //프로퍼티
        public SkinMakeData SkinMakeData => _skinMakeData;
        public bool IsAlreadyHave => _isAlreadyHave;

        //인스펙터 변수
        [SerializeField]
        private Image _skinImage = null;
        [SerializeField]
        private Image _alreadyHaveImage = null;
        [SerializeField]
        private TextMeshProUGUI _skinName = null;
        [SerializeField]
        private Button _skinButton = null;

        //스킨 데이터
        private SkinMakeData _skinMakeData = null;

        //스킨메이커커맨드
        private SkinMakerComponent _skinMakerCommand = null;

        //변수
        private bool _isAlreadyHave = false;

        /// <summary>
        /// 스킨 데이터 설정
        /// </summary>
        /// <param name="skinMakeData"></param>
        public void SetSkinData(SkinMakeData skinMakeData, bool isAlreadyHave, SkinMakerComponent skinMakerCommand)
        {
            _skinMakeData = skinMakeData;
            _isAlreadyHave = isAlreadyHave;
            _skinImage.sprite = _skinMakeData.sprite;
            if (_isAlreadyHave)
			{
                _skinImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
                _alreadyHaveImage.gameObject.SetActive(true);
            }
            else
			{
                _skinImage.color = new Color(1f, 1f, 1f, 1);
                _alreadyHaveImage.gameObject.SetActive(false);
			}
            _skinName.text = _skinMakeData.skinName;
            _skinMakerCommand = skinMakerCommand;
            _skinButton.onClick.AddListener(() => skinMakerCommand.SetSkinMakeButtonAndBoxs(skinMakeData, isAlreadyHave));
        }
    }
}