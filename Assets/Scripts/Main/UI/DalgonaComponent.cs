using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Main.Deck;
public class DalgonaComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _dalgonaText = null;

    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
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
        _dalgonaText.text = userSaveData._dalgona.ToString();
    }
}
