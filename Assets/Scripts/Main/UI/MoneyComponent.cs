using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Main.Deck;
public class MoneyComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _moneyText = null;

    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
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
        _moneyText.text = userSaveData._money.ToString();
    }

}
