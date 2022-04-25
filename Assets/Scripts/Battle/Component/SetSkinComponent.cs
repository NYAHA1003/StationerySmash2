using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

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

    public async void Start()
    {
        await _playerPencilCase.PencilCasedataBase.pencilCaseData.skinData.SetSkin(_playerPencilCase.PencilCasedataBase.pencilCaseData.skinData._skinType);
        await _enemyPencilCase.PencilCasedataBase.pencilCaseData.skinData.SetSkin(_enemyPencilCase.PencilCasedataBase.pencilCaseData.skinData._skinType);
        for (int i = 0; i < _cardDeckSO.cardDatas.Count; i++)
        {
            CardData cardData = _cardDeckSO.cardDatas[i];
            await cardData.skinData.SetSkin(cardData.skinData._skinType);
        }

        _isAllSetSkin = true;
    }
}
