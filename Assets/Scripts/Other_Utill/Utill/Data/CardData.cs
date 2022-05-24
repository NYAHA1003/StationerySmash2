using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Battle.Starategy;

[System.Serializable]
public class CardData : IDeepCopy<CardData>
{
    public CardNamingType _cardNamingType;
    public CardType cardType;
    public StrategyType starategyType;
    public string card_Name;
    public string card_Description;
    public int card_Cost;
    public SkinData _skinData;
    public int level;

    //유닛소환형
    //[ShowWhen("cardType", new object[] { CardType.SummonUnit, CardType.SummonTrap, CardType.Installation })]
    public UnitData unitData;

    /// <summary>
    /// CardData를 깊은 복사해서 반환함
    /// </summary>
    /// <returns></returns>
	public CardData DeepCopy()
    {
        CardData cardData = new CardData
        {
            _cardNamingType = this._cardNamingType,
            cardType = this.cardType,
            card_Name = this.card_Name,
            card_Cost = this.card_Cost,
            level = this.level,
        };

        //유닛데이터 레벨에 따라 변경
        cardData.unitData.CalculationLevel(level);

        return cardData;
    }
}
