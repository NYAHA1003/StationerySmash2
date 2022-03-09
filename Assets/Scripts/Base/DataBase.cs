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

    //������
    public StrategyData strategyData;

    //���ּ�ȯ��
    public UnitData unitData;

    //���� ��ȯ��

    //��ġ��
}

[System.Serializable]
public class UnitData
{

    public int unit_Hp;
    public Sprite unit_Sprite;
    public int unit_Weight;
    public int knockback;
    public float dir;
    public float accuracy;
    public float moveSpeed;
    public int damage;
    public float attackSpeed;
    public float range;
    public UnitType unitType;
    public AtkType atkType;
    public float[] unitablityData;
}


[System.Serializable]
public class StrategyData
{
    public Sprite strategy_Sprite;
}
