using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Main.Deck;
using Utill.Tool;
public class MoneyComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _moneyText = null;

    private int _currentMoney = 0;
    private int _previousMoney = 0;


    public void Awake()
    {
        UserSaveManagerSO.AddObserver(this);
        _previousMoney = UserSaveManagerSO.UserSaveData._money;
        SetMoneyText();
    }

    public void Notify()
    {
        _currentMoney = UserSaveManagerSO.UserSaveData._money;

        SetMoneyText();
        StartCoroutine(UpCountingMoney());
    }

    /// <summary>
    /// 돈 텍스트 값 수정
    /// </summary>
    public void SetMoneyText()
    {
        _moneyText.text = _previousMoney.ToString();
    }

    /// <summary>
    /// 돈 업데이트
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpCountingMoney()
    {
        float interval = 0.05f;
        while (_previousMoney < _currentMoney)
        {
            _previousMoney++;
            SetMoneyText();

            yield return new WaitForSeconds(interval);
        }
    }

}
