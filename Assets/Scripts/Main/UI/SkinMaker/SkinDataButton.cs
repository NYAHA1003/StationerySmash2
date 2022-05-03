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
    /// ��Ų ������ ���� ��ư Ŭ����
    /// </summary>
    public class SkinDataButton : MonoBehaviour
    {
        //������Ƽ
        public SkinMakeData SkinMakeData => _skinMakeData;
        public bool IsAlreadyHave => _isAlreadyHave;

        //�ν����� ����
        [SerializeField]
        private Image _skinImage = null;
        [SerializeField]
        private Image _alreadyHaveImage = null;
        [SerializeField]
        private TextMeshProUGUI _skinName = null;
        [SerializeField]
        private Button _skinButton = null;

        //��Ų ������
        private SkinMakeData _skinMakeData = null;

        //��Ų����ĿĿ�ǵ�
        private SkinMakerComponent _skinMakerCommand = null;

        //����
        private bool _isAlreadyHave = false;

        /// <summary>
        /// ��Ų ������ ����
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