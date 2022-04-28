using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;

public class ProfileComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private SaveDataSO _saveDataSO = null;
    [SerializeField]
    private Image _profileImage = null;

    public void Start()
    {
        _saveDataSO.AddObserver(this);
    }

    public void Notify(ref UserSaveData userSaveData)
    {
        SetMoneyText(ref userSaveData);
    }

    /// <summary>
    /// 돈 텍스트 값 수정
    /// </summary>
    public void SetMoneyText(ref UserSaveData userSaveData)
    {
        //어드레서블로 프로필 이미지를 가져온다.
        //_profileImage.sprite = userSaveData._currentProfileType;
    }
}

