using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

[System.Serializable]
public class SetSkinComponent : MonoBehaviour
{
    //������Ƽ 
    public bool IsAllSetSkin => _isAllSetSkin;
    
    //����
    private bool _isAllSetSkin = false;

    //�ν����� ����
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
    /// ���� �ִ� �͸� ��������Ʈ���� �ҷ��´�
    /// </summary>
    private async void SetSkinNowHave()
    {
        //��Ʋ �׽�Ʈ ������ ���
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
    /// ��� ��������Ʈ�� �ҷ��´�
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
