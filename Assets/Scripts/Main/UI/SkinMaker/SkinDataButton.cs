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

        //�ν����� ����
        [SerializeField]
        private Image _skinImage = null;
        [SerializeField]
        private TextMeshProUGUI _skinName = null;
        [SerializeField]
        private Button _skinButton = null;

        //��Ų ������
        private SkinMakeData _skinMakeData = null;

        //��Ų����ĿĿ�ǵ�
        private SkinMakerComponent _skinMakerCommand = null;

        /// <summary>
        /// ��Ų ������ ����
        /// </summary>
        /// <param name="skinMakeData"></param>
        public void SetSkinData(SkinMakeData skinMakeData, SkinMakerComponent skinMakerCommand)
        {
            _skinMakeData = skinMakeData;
            _skinImage.sprite = _skinMakeData.sprite;
            _skinName.text = _skinMakeData.skinName;
            _skinMakerCommand = skinMakerCommand;
            _skinButton.onClick.AddListener(() => skinMakerCommand.SetSkinMakeButtonAndBoxs(skinMakeData));
        }
    }
}