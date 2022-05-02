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
    /// �� �ؽ�Ʈ �� ����
    /// </summary>
    public void SetMoneyText(ref UserSaveData userSaveData)
    {
        //��巹����� ������ �̹����� �����´�.
        //_profileImage.sprite = userSaveData._currentProfileType;
    }
}

