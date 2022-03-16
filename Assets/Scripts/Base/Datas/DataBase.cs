using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill;

[System.Serializable]
public class DataBase
{
    public CardType cardType;
    public string card_Name;
    public int card_Cost;
    public Sprite card_Sprite;

    //실행형
    [ShowWhen("cardType", new object[] { CardType.Execute, CardType.SummonTrap, CardType.Installation })]
    public StarategyData strategyData;

    //유닛소환형
    [ShowWhen("cardType", new object[] { CardType.SummonUnit, CardType.SummonTrap, CardType.Installation })]
    public UnitData unitData;

    //함정 소환형

    //설치형
}

[System.Serializable]
public class UnitData
{
    public int unit_Hp;
    public int unit_Weight;
    public int knockback;
    public float dir;
    public float accuracy;
    public float moveSpeed;
    public int damage;
    public float attackSpeed;
    public float range;
    public CollideData colideData;
    public UnitType unitType;
    public AtkType atkType;
    public float[] unitablityData;
}


[System.Serializable]
public class StarategyData
{
    public StarategyType starategyType;
    public Starategy_State starategy_State;

    public StarategyData()
    {
        starategy_State = new Starategy_State();

        switch (starategyType)
        {
            case StarategyType.a:
                break;
            case StarategyType.b:
                break;
            case StarategyType.c:
                break;
            case StarategyType.d:
                break;
        }
    }
}
