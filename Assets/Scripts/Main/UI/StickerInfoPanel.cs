using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using TMPro;

public class StickerInfoPanel : MonoBehaviour
{
    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _decriptionText = null;


    /// <summary>
    /// ��ƼĿ â�� �Է¹��� ��ƼĿ �����Ϳ� �°� �����ϰ� ����
    /// </summary>
    /// <param name="skinData"></param>
    public void OnSetSkickerPanel(StickerData stickerData)
    {
        _image.sprite = SkinData.GetSkin(stickerData._skinType);
        _nameText.text = stickerData._name;
        _decriptionText.text = stickerData._decription;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ��ƼĿ â�� �ݴ´�
    /// </summary>
    /// <param name="skinData"></param>
    public void OnCloseStickerPanel()
    {
        gameObject.SetActive(false);
    }
}
