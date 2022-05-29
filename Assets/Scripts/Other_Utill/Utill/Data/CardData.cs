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
    public CardType _cardType;
    public StrategyType _starategyType;
    public UnitType _unitType;
    public string _name;
    public string _description;
    public int _cost;
    public SkinData _skinData;
    public int _level;

    /// <summary>
    /// CardData를 깊은 복사해서 반환함
    /// </summary>
    /// <returns></returns>
	public CardData DeepCopy()
    {
        CardData cardData = new CardData
        {
            _cardNamingType = this._cardNamingType,
            _cardType = this._cardType,
            _name = this._name,
            _cost = this._cost,
            _level = this._level,
        };

        return cardData;
    }
}
