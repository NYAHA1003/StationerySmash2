using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill;

public class SkinButton : MonoBehaviour
{
    //������Ƽ
    public SkinMakeData SkinMakeData => _skinMakeData;

    //�ν����� ����
    [SerializeField]
    private Image _skinImage = null;
    [SerializeField]
    private TextMeshProUGUI _skinName = null;

    //��Ų ������
    private SkinMakeData _skinMakeData = null;

    //��Ų����ĿĿ�ǵ�
    private SkinMakerCommand _skinMakerCommand = null;

    /// <summary>
    /// ��Ų ������ ����
    /// </summary>
    /// <param name="skinMakeData"></param>
    public void SetSkinData(SkinMakeData skinMakeData, SkinMakerCommand skinMakerCommand)
    {
        _skinMakeData = skinMakeData;
        _skinImage.sprite = _skinMakeData.sprite;
        _skinName.text = _skinMakeData.skinName;
        _skinMakerCommand = skinMakerCommand;
        //��ư���ٰ� �ֵ帮���� �߰�
        //skinMakerCommand.SetSkinMake()
        //�� �Լ� �߰��ϸ� ��.
    }
}
