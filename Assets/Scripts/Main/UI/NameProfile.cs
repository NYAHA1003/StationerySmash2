using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
public class NameProfile : MonoBehaviour, IUserData
{
    [SerializeField]
    private SaveDataSO _saveDataSO = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;

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
        _nameText.text = userSaveData._name;
    }
}
