using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill;

[System.Serializable]
public class CardData
{
    public CardType cardType;
    public string card_Name;
    public int card_Cost;
    public Sprite card_Sprite;
    public int level;

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
    public StickerData stickerData;
    public UnitType unitType;
    public float[] unitablityData;
}


[System.Serializable]
public class StarategyData
{
    public StarategyType starategyType;
    public AbstractStarategy starategy_State;
    public float[] starategyablityData = new float[0];

    public void Set_State()
    {

        switch (starategyType)
        {
            case StarategyType.None:
                break;
            case StarategyType.CostUp:
                starategy_State = new Starategy_CostUp();
                break;
            case StarategyType.InstallCandy:
                starategy_State = new Starategy_Candy();
                break;
            case StarategyType.InstallSlowdown:
                starategy_State = new Starategy_Slowdown();
                break;
            case StarategyType.InstallRage:
                starategy_State = new Starategy_Rage();
                break;
        }
    }
}
