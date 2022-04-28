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
    private SaveDataSO _saveDataSO = null;
    [SerializeField]
    private TextMeshProUGUI _dalgonaText = null;

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
        _dalgonaText.text = userSaveData._dalgona.ToString();
    }
}
