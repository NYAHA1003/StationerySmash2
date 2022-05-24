using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Battle.Starategy;

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
    /// 레벨에 따른 유닛 스탯 증가
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
