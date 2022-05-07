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
    //�ν����� ��������
    [SerializeField]
    private SkinListSO _skinListSO = null;

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
