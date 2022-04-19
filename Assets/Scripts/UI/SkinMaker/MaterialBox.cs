using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill;

[System.Serializable]
public class MaterialBox : MonoBehaviour
{
    //프로퍼티
    public MaterialData MaterialData => _materialData;

    //인스펙터 변수
    [SerializeField]
    private Image _materialImage = null;
    [SerializeField]
    private TextMeshProUGUI _materialNameText = null;
    [SerializeField]
    private TextMeshProUGUI _materialCountText = null;

    //변수
    public MaterialData _materialData = null;

    /// <summary>
    /// 재료 설정
    /// </summary>
    /// <param name="materialData"></param>
    public void SetMaterial(MaterialData materialData)
    {
        _materialData = materialData;
        _materialImage.sprite = materialData._sprite;
        _materialNameText.text = materialData.name;
        _materialCountText.text = materialData._count.ToString();
    }



}
