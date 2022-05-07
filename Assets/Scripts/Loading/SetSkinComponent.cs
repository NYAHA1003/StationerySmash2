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
    //인스펙터 참조변수
    [SerializeField]
    private SkinListSO _skinListSO = null;

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
        int cardTypeCount = _skinListSO._cardNamingSkins.Count;
        
        for (int i = 0; i < cardTypeCount; i++)
        {
            CardNamingSkins cardNamingSkins = _skinListSO._cardNamingSkins[i];
            int skinCount = cardNamingSkins._skinDatas.Count;
            
            for(int j = 0; j < skinCount; j++)
			{
                SkinData skinData = cardNamingSkins._skinDatas[j];
                skinData.AddSkinDataIntCardDictionary(cardNamingSkins._cardNamingType);
                await SkinData.SetSkinStaticAsync(skinData._skinType);
			}
        }

        _isAllSetSkin = true;
    }
}
