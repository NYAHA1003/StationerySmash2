using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Utill;

public class CardInfoPanel : MonoBehaviour
{
    //카드 스탯 텍스트들
    //유닛일 때만 사용
    private GameObject _unitStatTexts = null;
    private TextMeshProUGUI _hpText = null;
    private TextMeshProUGUI _attackText = null;
    private TextMeshProUGUI _attackSpeedText = null;
    private TextMeshProUGUI _moveSpeedText = null;
    private TextMeshProUGUI _weightText = null;

    //카드 설명창 
    private TextMeshProUGUI _nameText = null;
    private TextMeshProUGUI _descriptionText = null;

    //카드 이미지
    private Image _cardImage;

    //가지고 있는 스킨
    //인벤토리 가지고 와야함

    //스티커 착용창
    //유닛일 때만 사용

    //미리보기창


    private CardData _selectCardData;

    /// <summary>
    /// 카드데이터 설정
    /// </summary>
    public void SetCardInfoPanel(CardData cardData)
    {
        _selectCardData = cardData;

        //카드 타입에 따라 설명창 설정
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

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    public void SetCardSummonUnit(CardData cardData)
    {
        _unitStatTexts.SetActive(true);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;

        //스탯 텍스트 설정
        _hpText.text = cardData.unitData.unit_Hp.ToString();
        _attackText.text = cardData.unitData.damage.ToString();
        _attackSpeedText.text = cardData.unitData.attackSpeed.ToString();
        _moveSpeedText.text = cardData.unitData.moveSpeed.ToString();
        _weightText.text = cardData.unitData.unit_Weight.ToString();
    }
    public void SetCardSummonTrap(CardData cardData)
    {
        _unitStatTexts.SetActive(false);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    public void SetCardInstallation(CardData cardData)
    {
        _unitStatTexts.SetActive(false);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData.cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
}
