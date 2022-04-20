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
