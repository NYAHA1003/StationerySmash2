using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
public class NameProfileComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _nameText = null;

    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
    }

    public void Notify(ref UserSaveData userSaveData)
    {
        SetNameText(ref userSaveData);
    }

    /// <summary>
    /// 이름 텍스트 값 수정
    /// </summary>
    public void SetNameText(ref UserSaveData userSaveData)
    {
        _nameText.text = userSaveData._name;
    }
}
