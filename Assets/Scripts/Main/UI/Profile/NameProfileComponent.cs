using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
using Utill.Tool;
public class NameProfileComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _nameText = null;

    public void Awake()
    {
        SaveManager.Instance.SaveData.AddObserver(this);
    }

    public void Notify()
    {
        SetNameText();
    }

    /// <summary>
    /// 이름 텍스트 값 수정
    /// </summary>
    public void SetNameText()
    {
        _nameText.text = UserSaveManagerSO.UserSaveData._name;
    }
}
