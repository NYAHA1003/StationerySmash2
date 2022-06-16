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
    public int _count;

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
            _description = this._description,
            _cost = this._cost,
            _level = this._level,
            _unitType = this._unitType,
            _starategyType = this._starategyType,
            _skinData = new SkinData()
            {
                _effectType = this._skinData._effectType,
                _skinType = this._skinData._skinType
            }

        };

        return cardData;
    }


}

/// <summary>
/// 카드 데이터가 같은지 체크함
/// </summary>
public class CardDataComparer : IEqualityComparer<CardData>
{
    public bool Equals(CardData x, CardData y)
    {
        // 참조 값 동일한지 비교
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }

        // 객체 참조 중 하나라도 null이면 false 반환 
        if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
        {
            return false;
        }

        // 객체의 속성을 하나씩 비교
        return x._cardNamingType == y._cardNamingType;
    }

    public int GetHashCode(CardData obj)
    {
        // 객체가 존재하는지 체크
        if (obj == null)
        {
            return 0;
        }

        // 문자열은 null이 가능하므로 예외 처리
        int NameHashCode = obj._cardNamingType == CardNamingType.None ? 0 : obj._cardNamingType.GetHashCode();

        return NameHashCode;
    }
}
