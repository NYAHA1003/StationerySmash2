using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static LanguageSingleton;

public class TextSetting : MonoBehaviour
{
    //색깔 변경 기능
    public Color textColor;
    //외곽선 수정 기능
    public Color outLineColor;
    [Range(0.0f, 1f)]
    public float outline;

    private TextMeshProUGUI tmpGUI;

    private void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }


    [ContextMenu("텍스트 설정 적용하기")]
    public void Set_Text()
    {
        if(tmpGUI == null)
        {
            tmpGUI = GetComponent<TextMeshProUGUI>();
        }
        tmpGUI.outlineColor = outLineColor;
        tmpGUI.material.SetColor("_OutlineColor", outLineColor);
        tmpGUI.material.SetFloat("_FaceDilate", outline - 0.1f);
        tmpGUI.material.SetFloat("_OutlineWidth", outline);
    }

}
