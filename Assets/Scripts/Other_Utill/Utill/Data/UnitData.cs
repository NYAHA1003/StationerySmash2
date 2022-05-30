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
    public int _hp;
    public int _weight;
    public int _knockback;
    public float _dir;
    public float _accuracy;
    public float _moveSpeed;
    public int _damage;
    public float _attackSpeed;
    public float _range;
    public CollideData _colideData;
    public StickerType _stickerType;
    public AttackType _attackType;
    public UnitType _unitType;
    public float[] _unitablityData;

    /// <summary>
    /// 레벨에 따른 유닛 스탯 증가
    /// </summary>
    /// <param name="level"></param>
    public void CalculationLevel(int level)
    {
        _hp *= level;
        _knockback *= level;
        _moveSpeed *= level;
        _damage *= level;
        _attackSpeed *= level;
    }
}
