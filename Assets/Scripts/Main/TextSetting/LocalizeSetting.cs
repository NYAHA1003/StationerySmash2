using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Main.TextSetting
{
    using static LanguageSingleton;

    namespace Main.TextSetting
    {
        public class LocalizeSetting : MonoBehaviour
        {
            Dropdown dropdown;

            private void Start()
            {
                dropdown = GetComponent<Dropdown>();
                if (dropdown.options.Count != LanguageSingleton._instance.Langs.Count)     //����ٿ��� �ɼ� ���� ���� �����Ϳ� �ִ� ����� ���� �ٸ��ٸ� 
                    SetLangOption();
                dropdown.onValueChanged.AddListener((d) => LanguageSingleton._instance.SetLangIndex(dropdown.value));  //����ٿ��� value�� ����Ǹ� LanguageSingleton���� ���� ��� �ε��� ���� �� ����

            }

            private void OnDestroy()
            {
                LanguageSingleton._instance.LocalizeSettingChanged -= LocalizeSettingChanged;
            }

            void SetLangOption()
            {
                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();


                //����ٿ��� �ɼǿ� �� ����Ʈ�� ����� ����ȭ �̸� �߰�
                for (int i = 0; i < LanguageSingleton._instance.Langs.Count; i++)
                {
                    optionDatas.Add(new Dropdown.OptionData() { text = LanguageSingleton._instance.Langs[i].langLocalize });
                }

                dropdown.options = optionDatas;
                Debug.Log("��Ӵٿ� ������Ʈ plz");
            }

            void LocalizeSettingChanged()
            {
                dropdown.value = LanguageSingleton._instance.curLangIndex; //����ٿ��� ���� value�� ���� ���� �ٲ��ش�. 
            }
        }
    }
}