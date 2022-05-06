using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Card;
using Main.Deck;

[System.Serializable]
public class SetSkinComponent : MonoBehaviour
{
    //������Ƽ 
    public bool IsAllSetSkin => _isAllSetSkin;
    
    //����
    private bool _isAllSetSkin = false;


    public  void Start()
    {
        SetSkinAll();
    }



    /// <summary>
    /// ��� ��������Ʈ�� �ҷ��´�
    /// </summary>
    private async void SetSkinAll()
    {
        var skinTypes = System.Enum.GetValues(typeof(SkinType));
        int skinTypeCount = skinTypes.Length;
        for (int i = 0; i < skinTypeCount; i++)
        {
            await SkinData.SetSkinStatic((SkinType)skinTypes.GetValue(i));
        }

        _isAllSetSkin = true;
    }
}
