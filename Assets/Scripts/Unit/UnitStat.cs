using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ������Ʈ
/// </summary>
public class UnitStat
{
    //������Ƽ

    public int AttackPercent => _attackPercent; //���ݷ� �ۼ�Ʈ
    public int MoveSpeedPercent => _moveSpeedPercent;//�̵��ӵ� �ۼ�Ʈ
    public int AttackSpeedPercent => _attackSpeedPercent;//���ݼӵ� �ۼ�Ʈ
    public int RangePercent => _rangePercent;//�����Ÿ� �ۼ�Ʈ
    public int AccuracyPercent => _accuracyPercent;//���߷� �ۼ�Ʈ
    public int WeightPercent => _weightPercent;//���� �ۼ�Ʈ
    public int KnockbackPercent => _knockbackPercent;//�˹� �ۼ�Ʈ
    public int DamagedPercent => _damagedPercent;//�˹� �ۼ�Ʈ
    public int Hp => _hp;//ü�� �ۼ�Ʈ
    public int MaxHp => _maxHp;//�ִ�ü�� �ۼ�Ʈ
    public int Grade => _grade; //���� �ܰ�
    public float AttackDelay => _attackDelay;//���� ������

    //����
    private int _attackPercent = 100;
    private int _moveSpeedPercent = 100;
    private int _attackSpeedPercent = 100;
    private int _rangePercent = 100;
    private int _accuracyPercent = 100;
    private int _weightPercent = 100;
    private int _knockbackPercent = 100;
    private int _damagedPercent = 100;
    private int _hp = 0;
    private int _maxHp = 0;
    private int _weight = 0;
    private int _grade = 1;
    private float _attackDelay = 0;

    //���� ����
    private UnitData _unitData = null;

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
        _grade = grade;
        _moveSpeedPercent = 100 * grade;
        _attackSpeedPercent = 100 * grade;
        _attackPercent = 100 * grade;
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
    public int Return_Attack()
    {
        return Mathf.RoundToInt(_unitData.damage * (float)_attackPercent / 100);
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
    public void IncreaseAttackPercent(int percent)
    {
        _attackPercent += percent;
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
    /// <summary>
    /// ������ �޴·� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseDamagedPercent(int percent)
    {
        _damagedPercent += percent;
    }

    /// <summary>
    /// ���� ������ 0���� �ʱ�ȭ
    /// </summary>
    public void ResetAttackDelay()
    {
        _attackDelay = 0;
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="delay"></param>
    public void SetAttackDelay(float delay)
    {
        _attackDelay = delay;
    }

}
