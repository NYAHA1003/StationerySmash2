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
                if (dropdown.options.Count != LanguageSingleton.Instance.Langs.Count)     //드랍다운의 옵션 수와 현재 데이터에 있는 언어의 수가 다르다면 
                    SetLangOption();
                dropdown.onValueChanged.AddListener((d) => LanguageSingleton.Instance.SetLangIndex(dropdown.value));  //드랍다운의 value가 변경되면 LanguageSingleton에서 현재 언어 인덱스 변경 및 저장

            }

            private void OnDestroy()
            {
                LanguageSingleton.Instance.LocalizeSettingChanged -= LocalizeSettingChanged;
            }

            void SetLangOption()
            {
                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();


                //드랍다운의 옵션에 들어갈 리스트에 언어의 현지화 이름 추가
                for (int i = 0; i < LanguageSingleton.Instance.Langs.Count; i++)
                {
                    optionDatas.Add(new Dropdown.OptionData() { text = LanguageSingleton.Instance.Langs[i].langLocalize });
                }

                dropdown.options = optionDatas;
                Debug.Log("드롭다운 업데이트 plz");
            }

            void LocalizeSettingChanged()
            {
                dropdown.value = LanguageSingleton.Instance.curLangIndex; //드랍다운의 현재 value를 현재 언어로 바꿔준다. 
            }
        }
    }
}