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
    /// 스티커 창을 입력받은 스티커 데이터에 맞게 설정하고 연다
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
    /// 스티커 창을 닫는다
    /// </summary>
    /// <param name="skinData"></param>
    public void OnCloseStickerPanel()
    {
        gameObject.SetActive(false);
    }
}
