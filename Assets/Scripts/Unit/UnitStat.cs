using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStat
{
    public UnitData _unitData;
    public int _damagePercent { get; private set; } = 100;
    public int _moveSpeedPercent { get; private set; } = 100;
    public int _attackSpeedPercent { get; private set; } = 100;
    public int _rangePercent { get; private set; } = 100;
    public int _accuracyPercent { get; private set; } = 100;
    public int _weightPercent { get; private set; } = 100;
    public int _knockbackPercent { get; private set; } = 100;
    public int _hp { get; private set; } = 0;
    public int _maxHp { get; private set; } = 0;
    public int _weight { get; private set; } = 0;

    /// <summary>
    /// 유닛 데이터 설정
    /// </summary>
    /// <param name="unitData"></param>
    public void SetUnitData(UnitData unitData)
    {
        _unitData = unitData;
    }
    /// <summary>
    /// 단계에 따른 스탯 증가
    /// </summary>
    /// <param name="grade"></param>
    public void SetGradeStat(int grade)
    {
        _moveSpeedPercent = 100 * grade;
        _attackSpeedPercent = 100 * grade;
        _damagePercent = 100 * grade;
        _maxHp = _unitData.unit_Hp * grade;
        _hp = _maxHp;
    }
    /// <summary>
    /// 무게 설정
    /// </summary>
    public void SetWeight()
    {
        _weight = _unitData.unit_Weight;
    }
    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public void SubtractHP(int damage)
    {
        _hp -= damage;
    }

    /// <summary>
    /// 공격력 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Damage()
    {
        return Mathf.RoundToInt(_unitData.damage * (float)_damagePercent / 100);
    }
    /// <summary>
    /// 이동속도 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_MoveSpeed()
    {
        return _unitData.moveSpeed * (float)_moveSpeedPercent / 100;
    }
    /// <summary>
    /// 공격속도 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_AttackSpeed()
    {
        return _unitData.attackSpeed * (float)_attackSpeedPercent / 100;
    }
    /// <summary>
    /// 사거리 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_Range()
    {
        return _unitData.range * (float)_rangePercent / 100;
    }
    /// <summary>
    /// 무게 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Weight()
    {
        return Mathf.RoundToInt(_unitData.unit_Weight * (float)_weightPercent / 100);
    }
    /// <summary>
    /// 명중률 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_Accuracy()
    {
        return _unitData.accuracy * (float)_accuracyPercent / 100;
    }
    /// <summary>
    /// 넉백 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(_unitData.knockback * (float)_knockbackPercent / 100);
    }

    /// <summary>
    /// 공격력 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseDamagePercent(int percent)
    {
        _damagePercent += percent;
    }
    /// <summary>
    /// 이동속도 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseMoveSpeedPercent(int percent)
    {
        _moveSpeedPercent += percent;
    }
    /// <summary>
    /// 공격속도 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseAttackSpeedPercent(int percent)
    {
        _attackSpeedPercent += percent;
    }
    /// <summary>
    /// 사정거리 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseRangePercent(int percent)
    {
        _rangePercent += percent;
    }
    /// <summary>
    /// 명중률 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseAccuracyPercent(int percent)
    {
        _accuracyPercent += percent;
    }
    /// <summary>
    /// 무게 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseWeightPercent(int percent)
    {
        _weightPercent += percent;
    }
    /// <summary>
    /// 넉백 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseKnockBackPercent(int percent)
    {
        _knockbackPercent += percent;
    }

}
