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
    //프로퍼티 
    public bool IsAllSetSkin => _isAllSetSkin;
    
    //변수
    private bool _isAllSetSkin = false;


    public  void Start()
    {
        SetSkinAll();
    }



    /// <summary>
    /// 모든 스프라이트를 불러온다
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
