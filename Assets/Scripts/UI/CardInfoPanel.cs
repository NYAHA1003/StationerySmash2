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
    private GameObject _unitStatTexts = null;
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

        //ī�� Ÿ�Կ� ���� ����â ����
        switch (_selectCardData.cardType)
        {
            case CardType.Execute:
                SetCardExecute(cardData);
                break;
            case CardType.SummonUnit:
                SetCardSummonUnit(cardData);
                break;
            case CardType.SummonTrap:
                SetCardSummonTrap(cardData);
                break;
            case CardType.Installation:
                SetCardInstallation(cardData);
                break;
        }
    }

    public void SetCardExecute(CardData cardData)
    {
        _unitStatTexts.SetActive(false);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    public void SetCardSummonUnit(CardData cardData)
    {
        _unitStatTexts.SetActive(true);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;

        //���� �ؽ�Ʈ ����
        _hpText.text = cardData.unitData.unit_Hp.ToString();
        _attackText.text = cardData.unitData.damage.ToString();
        _attackSpeedText.text = cardData.unitData.attackSpeed.ToString();
        _moveSpeedText.text = cardData.unitData.moveSpeed.ToString();
        _weightText.text = cardData.unitData.unit_Weight.ToString();
    }
    public void SetCardSummonTrap(CardData cardData)
    {
        _unitStatTexts.SetActive(false);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    public void SetCardInstallation(CardData cardData)
    {
        _unitStatTexts.SetActive(false);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
}
