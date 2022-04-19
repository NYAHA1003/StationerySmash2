using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill;

[System.Serializable]
public class MaterialBox : MonoBehaviour
{
    //������Ƽ
    public MaterialData MaterialData => _materialData;

    //�ν����� ����
    [SerializeField]
    private Image _materialImage = null;
    [SerializeField]
    private TextMeshProUGUI _materialNameText = null;
    [SerializeField]
    private TextMeshProUGUI _materialCountText = null;

    //����
    public MaterialData _materialData = null;

    /// <summary>
    /// ��� ����
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
