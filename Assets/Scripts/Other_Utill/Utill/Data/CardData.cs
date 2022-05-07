using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Battle.Starategy;

[System.Serializable]
public class CardData
{
    public CardNamingType _cardNamingType;
    public CardType cardType;
    public string card_Name;
    public string card_Description;
    public int card_Cost;
    public SkinData skinData;
    public int level;

    //������
    [ShowWhen("cardType", new object[] { CardType.Execute, CardType.SummonTrap, CardType.Installation })]
    public StarategyData strategyData;

    //���ּ�ȯ��
    [ShowWhen("cardType", new object[] { CardType.SummonUnit, CardType.SummonTrap, CardType.Installation })]
    public UnitData unitData;

    //���� ��ȯ��

    //��ġ��

    /// <summary>
    /// ���� ���� ���� & ������ ���� ��ġ ����
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public CardData DeepCopy(int level, SkinType skinType)
    {
        CardData cardData = new CardData
        {
            cardType = this.cardType,
            card_Name = this.card_Name,
            card_Cost = this.card_Cost,
            level = level,
            skinData = new SkinData
            {       
                _skinType = skinType,
                _effectType = this.skinData._effectType,
            },
            strategyData = new StarategyData
            {
                starategyType = this.strategyData.starategyType,
                starategy_State = this.strategyData.starategy_State,
                starategyablityData = this.strategyData.starategyablityData,
            },
            unitData = new UnitData
            {
                unit_Hp = this.unitData.unit_Hp,
                unit_Weight = this.unitData.unit_Weight,
                knockback = this.unitData.knockback,
                dir = this.unitData.dir,
                accuracy = this.unitData.accuracy,
                moveSpeed = this.unitData.moveSpeed,
                damage = this.unitData.damage,
                attackSpeed = this.unitData.attackSpeed,
                range = this.unitData.range,
                colideData = this.unitData.colideData,
                stickerData = this.unitData.stickerData,
                unitType = this.unitData.unitType,
                unitablityData = this.unitData.unitablityData,

            },
        };

        //���ֵ����� ������ ���� ����
        cardData.unitData.CalculationLevel(level);

        return cardData;
    }
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
    public AttackType attackType;
    public UnitType unitType;
    public float[] unitablityData;

    /// <summary>
    /// ������ ���� ���� ���� ����
    /// </summary>
    /// <param name="level"></param>
    public void CalculationLevel(int level)
    {
        unit_Hp *= level;
        knockback *= level;
        moveSpeed *= level;
        damage *= level;
        attackSpeed *= level;
    }
}


[System.Serializable]
public class StarategyData
{
    public StarategyType starategyType;
    public AbstractStarategy starategy_State;
    public float[] starategyablityData = new float[0];

    public void Set_State(params float[] values)
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
        //���� �ƴ� ��� �� ����� ���ش�.
        if(starategyType != StarategyType.None)
        {
            starategy_State.SetValuse(starategyablityData);
        }
    }
}
