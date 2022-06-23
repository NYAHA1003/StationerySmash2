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


    public void Start()
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
    /// �� �ؽ�Ʈ �� ����
    /// </summary>
    public void SetMoneyText()
    {
        _moneyText.text = _previousMoney.ToString();
    }

    /// <summary>
    /// �� ������Ʈ
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpCountingMoney()
    {
        int addMoney = (_currentMoney - _previousMoney) / 5;
        while (_previousMoney < _currentMoney)
        {
            _previousMoney += addMoney;
            SetMoneyText();

            yield return new WaitForSeconds(0.01f);
        }
        if(_previousMoney > _currentMoney)
        {
            _previousMoney = _currentMoney;
            SetMoneyText();
        }
    }

}
