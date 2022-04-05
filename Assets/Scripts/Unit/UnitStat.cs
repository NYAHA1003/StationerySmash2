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
    /// ���� ������ ����
    /// </summary>
    /// <param name="unitData"></param>
    public void SetUnitData(UnitData unitData)
    {
        _unitData = unitData;
    }
    /// <summary>
    /// �ܰ迡 ���� ���� ����
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
    /// ���� ����
    /// </summary>
    public void SetWeight()
    {
        _weight = _unitData.unit_Weight;
    }
    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public void SubtractHP(int damage)
    {
        _hp -= damage;
    }

    /// <summary>
    /// ���ݷ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Damage()
    {
        return Mathf.RoundToInt(_unitData.damage * (float)_damagePercent / 100);
    }
    /// <summary>
    /// �̵��ӵ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_MoveSpeed()
    {
        return _unitData.moveSpeed * (float)_moveSpeedPercent / 100;
    }
    /// <summary>
    /// ���ݼӵ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_AttackSpeed()
    {
        return _unitData.attackSpeed * (float)_attackSpeedPercent / 100;
    }
    /// <summary>
    /// ��Ÿ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_Range()
    {
        return _unitData.range * (float)_rangePercent / 100;
    }
    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Weight()
    {
        return Mathf.RoundToInt(_unitData.unit_Weight * (float)_weightPercent / 100);
    }
    /// <summary>
    /// ���߷� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_Accuracy()
    {
        return _unitData.accuracy * (float)_accuracyPercent / 100;
    }
    /// <summary>
    /// �˹� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(_unitData.knockback * (float)_knockbackPercent / 100);
    }

    /// <summary>
    /// ���ݷ� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseDamagePercent(int percent)
    {
        _damagePercent += percent;
    }
    /// <summary>
    /// �̵��ӵ� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseMoveSpeedPercent(int percent)
    {
        _moveSpeedPercent += percent;
    }
    /// <summary>
    /// ���ݼӵ� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseAttackSpeedPercent(int percent)
    {
        _attackSpeedPercent += percent;
    }
    /// <summary>
    /// �����Ÿ� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseRangePercent(int percent)
    {
        _rangePercent += percent;
    }
    /// <summary>
    /// ���߷� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseAccuracyPercent(int percent)
    {
        _accuracyPercent += percent;
    }
    /// <summary>
    /// ���� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseWeightPercent(int percent)
    {
        _weightPercent += percent;
    }
    /// <summary>
    /// �˹� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseKnockBackPercent(int percent)
    {
        _knockbackPercent += percent;
    }

}
