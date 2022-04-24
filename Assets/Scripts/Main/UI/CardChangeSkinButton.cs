using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill;

public class CardChangeSkinButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _skinNameText = null;
    [SerializeField]
    private Image _skinImage = null;
    
    /// <summary>
    /// ��Ų �̹��� ����
    /// </summary>
    /// <param name="skinData"></param>
    public void SetButtonImages(SkinData skinData)
    {
        _skinNameText.text = skinData._cardNamingType.ToString();
        _skinImage.sprite = SkinData.GetSkin(skinData._skinType);
    }
}
