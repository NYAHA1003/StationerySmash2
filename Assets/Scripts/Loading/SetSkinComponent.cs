using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

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
        SetSkinNowHave();
        //SetSkinAll();
    }

    /// <summary>
    /// 현재 있는 것만 스프라이트들을 불러온다
    /// </summary>
    private async void SetSkinNowHave()
    {
        //배틀 테스트 용으로 사용
        await _playerPencilCase.PencilCasedataBase.pencilCaseData.skinData.SetSkin(_playerPencilCase.PencilCasedataBase.pencilCaseData.skinData._skinType);
        await _enemyPencilCase.PencilCasedataBase.pencilCaseData.skinData.SetSkin(_enemyPencilCase.PencilCasedataBase.pencilCaseData.skinData._skinType);

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
        await _playerPencilCase.PencilCasedataBase.pencilCaseData.skinData.SetSkin(_playerPencilCase.PencilCasedataBase.pencilCaseData.skinData._skinType);
        await _enemyPencilCase.PencilCasedataBase.pencilCaseData.skinData.SetSkin(_enemyPencilCase.PencilCasedataBase.pencilCaseData.skinData._skinType);

        for (int i = 0; i < System.Enum.GetValues(typeof(SkinType)).Length; i++)
        {
            CardData cardData = _cardDeckSO.cardDatas[i];
            await cardData.skinData.SetSkin((SkinType)i);
        }

        _isAllSetSkin = true;
    }
}
