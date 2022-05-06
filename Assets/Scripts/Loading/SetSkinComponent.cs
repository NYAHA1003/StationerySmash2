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

    //인스펙터 변수
    [SerializeField]
    private CardDeckSO _cardDeckSO = null;
    [SerializeField]
    private PencilCaseDataSO _playerPencilCase = null;
    [SerializeField]
    private PencilCaseDataSO _enemyPencilCase = null;

    public  void Start()
    {
        SetSkinAll();
    }

    /// <summary>
    /// 현재 있는 것만 스프라이트들을 불러온다
    /// </summary>
    private async void SetSkinNowHave()
    {
        //배틀 테스트 용으로 사용
        for (int i = 0; i < _cardDeckSO.cardDatas.Count; i++)
        {
            CardData cardData = _cardDeckSO.cardDatas[i];
            await cardData.skinData.SetSkin(cardData.skinData._skinType);
        }

        _isAllSetSkin = true;
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
