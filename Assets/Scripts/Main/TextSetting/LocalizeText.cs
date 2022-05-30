using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Main.TextSetting
{
    using static LanguageSingleton;
    public class LocalizeText : MonoBehaviour
    {

        public string textKey;      //0��° ��(���� ������)�� �������� key�� ���� ���ڿ� ���� 
        public string[] dropdownKey;    //���� text�� �ƴ� ����ٿ��� ���

        private TextMeshProUGUI tmpGUI;

        private void Start()
        {
            tmpGUI = GetComponent<TextMeshProUGUI>();
            LocalizeChanged();
            LanguageSingleton.Instance.LocalizeChanged += LocalizeChanged;
        }

        private void OnDestroy()
        {
            LanguageSingleton.Instance.LocalizeChanged -= LocalizeChanged;
        }

        string Localize(string key) //� ���ڿ��� �ʿ����� key�� �Ű������� �޴´� 
        {
            int keyIndex = LanguageSingleton.Instance.Langs[0].value.FindIndex(x => x.ToLower() == key.ToLower()); //������ �Ǵ� 0�� �������� value Ž�� �� keyIndex�� ���ڿ� ����
            return LanguageSingleton.Instance.Langs[LanguageSingleton.Instance.curLangIndex].value[keyIndex];      //���� ��� �ε���, value�� key�� �������� ���ڿ��� ��ȯ�Ѵ�. 
        }

        void LocalizeChanged()
        {
            if (tmpGUI != null)
            {
                tmpGUI.text = Localize(textKey);  // Localize�� ��ȯ ������ �ؽ�Ʈ�� �ٲ��ش�.
                return;
            }

            if (GetComponent<Dropdown>() != null)
            {
                Dropdown dropdown = GetComponent<Dropdown>();
                dropdown.captionText.text = Localize(dropdownKey[dropdown.value]);

                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    dropdown.options[i].text = Localize(dropdownKey[i]);
                }
            }
        }
    }
}