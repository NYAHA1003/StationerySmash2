using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static LanguageSingleton;

public class TextSetting : MonoBehaviour
{
    //���� ���� ���
    //�ܰ��� ���� ���
    //�ܱ��� ���� ���


    private TextMeshProUGUI tmpGUI;

    private void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }

}
