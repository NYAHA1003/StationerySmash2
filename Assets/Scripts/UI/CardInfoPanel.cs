using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Utill;

public class CardInfoPanel : MonoBehaviour
{
    //ī�� ���� �ؽ�Ʈ��
    //������ ���� ���
    private TextMeshProUGUI _hpText = null;
    private TextMeshProUGUI _attackText = null;
    private TextMeshProUGUI _attackSpeedText = null;
    private TextMeshProUGUI _moveSpeedText = null;
    private TextMeshProUGUI _weightText = null;

    //ī�� ����â 
    private TextMeshProUGUI _nameText = null;
    private TextMeshProUGUI _descriptionText = null;

    //ī�� �̹���
    private Image _cardImage;

    //������ �ִ� ��Ų
    //�κ��丮 ������ �;���

    //��ƼĿ ����â
    //������ ���� ���

    //�̸�����â


    private CardData _selectCardData;

    /// <summary>
    /// ī�嵥���� ����
    /// </summary>
    public void SetCardInfoPanel(CardData cardData)
    {
        _selectCardData = cardData;

        switch (_selectCardData.cardType)
        {
            case CardType.Execute:
                break;
            case CardType.SummonUnit:
                break;
            case CardType.SummonTrap:
                break;
            case CardType.Installation:
                break;
        }
    }

    public void SetCardExecute(CardData cardData)
    {

    }
    public void SetCardSummonUnit(CardData cardData)
    {

    }
    public void SetCardSummonTrap(CardData cardData)
    {

    }
    public void SetCardInstallation(CardData cardData)
    {

    }
}
