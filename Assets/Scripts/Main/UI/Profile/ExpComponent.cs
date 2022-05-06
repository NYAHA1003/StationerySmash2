using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;

public class ExpComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private Image _expImage = null;
    [SerializeField]
    private TextMeshProUGUI _expText = null;

    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
    }

    public void Notify(ref UserSaveData userSaveData)
    {
        SetExpBar(ref userSaveData);
    }

    /// <summary>
    /// EXP¹Ù ¼öÁ¤
    /// </summary>
    public void SetExpBar(ref UserSaveData userSaveData)
    {
        _expText.text = $"{userSaveData._nowExp}/{(userSaveData._level * 100)}";
        _expImage.fillAmount = (float)userSaveData._nowExp / (userSaveData._level * 100); 
    }
}
