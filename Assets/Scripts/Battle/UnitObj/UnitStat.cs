using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ������Ʈ
/// </summary>
public class UnitStat
{
    public enum WeightGrade
	{
        TooLight,
        Light,
        Normal,
        Heavy,
        TooHeavy,
    }

    //������Ƽ
    public int AttackPercent => _attackPercent; //���ݷ� �ۼ�Ʈ
    public int MoveSpeedPercent => _moveSpeedPercent;//�̵��ӵ� �ۼ�Ʈ
    public int AttackSpeedPercent => _attackSpeedPercent;//���ݼӵ� �ۼ�Ʈ
    public int RangePercent => _rangePercent;//�����Ÿ� �ۼ�Ʈ
    public int AccuracyPercent => _accuracyPercent;//���߷� �ۼ�Ʈ
    public int WeightPercent => _weightPercent;//���� �ۼ�Ʈ
    public int KnockbackPercent => _knockbackPercent;//�˹� �ۼ�Ʈ
    public int DamagedPercent => _damagedPercent;// ������ �ۼ�Ʈ
    public int DamageDecrese => _damagedDecrese; // ������
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
    private int _damagedDecrese = 0;
    private int _hp = 0;
    private int _maxHp = 0;
    private int _weight = 0;
    private int _grade = 1;
    private float _attackDelay = 0;
    private int _bonusMoveSpeed = 0;
    private int _bonusAttackSpeed = 0;
    private int _bonusMaxHp = 0;
    private int _bonusAttack = 0;
    private int _bonusKnockback = 0;
    private int _bonusAccuracy = 0;
    private float _bonusRange = 0;
    private int _bonusWeight = 0;
    public bool _isInvincible = false;

    //���� ����
    private UnitData _unitData = null;

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="unitData"></param>
    /// <param name="grade"></param>
    public void ResetStat(UnitData unitData, int grade)
    {
        ResetBonusStat();
        ResetAttackDelay();
        SetUnitData(unitData);
        SetGradeStat(grade);
        SetWeight();
        SetAttackDelay(0);
    }

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
        float multiple = 0;
        float divid = 0;
        switch(grade)
		{
            case 0:
                multiple = 0;
                divid = 0;
                break;
            case 1:
                multiple = 1;
                divid = 1;
                break;
            case 2:
                multiple = 1.2f;
                divid = 0.8f;
                break;
            case 3:
                multiple = 1.5f;
                divid = 0.5f;
                break;
            case 4:
                multiple = 2f;
                divid = 0.3f;
                break;
        }

        _grade = grade;
        _moveSpeedPercent = (int)(100 * divid);
        _attackSpeedPercent = (int)(100 * multiple);
        _attackPercent = (int)(100 * multiple);
        _maxHp = (int)(_unitData._hp * multiple);
        _hp = _maxHp;
    }
    public void GradeUp(int grade)
    {
        _grade += grade;
    }
    public void Invincible()
    {
        _isInvincible = true;
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    public void SetWeight()
    {
        _weight = _unitData._weight;
    }
    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public void SubtractHP(int damage) //���� �ٲ����
    {
        if(_isInvincible)
        {
            _isInvincible = false;
            return;
        }
        _hp -= damage;
    }

    public void RecoveryHPPercent(int recovery)
    {
        _hp += _maxHp * (1 + recovery / 100);
    }


    /// <summary>
    /// ���ʽ� ���� �ʱ�ȭ
    /// </summary>
    public void ResetBonusStat()
    {
        _bonusMoveSpeed = 0;
        _bonusAttackSpeed = 0;
        _bonusMaxHp = 0;
        _bonusAttack = 0;
        _bonusKnockback = 0;
        _bonusAccuracy = 0;
        _bonusRange = 0;
        _bonusWeight = 0;
        _damagedDecrese = 0;
    }
    /// <summary>
    /// ���� ü���� ���� �ۼ�Ʈ �����ֱ�
    /// </summary>
    /// <param name="return"></param>
    public int LostHpPenrcent(int percent)
    {
        int lostHPpercent;
        lostHPpercent = (1 - _hp / _maxHp) * percent;
        return lostHPpercent;
    }

    /// <summary>
    /// ���ʽ� �̵� �ӵ� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusMoveSpeed(int add)
    {
        _bonusMoveSpeed += add;
    }

    /// <summary>
    /// ���ʽ� ���ݷ� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusAttack(int add)
    {
        _bonusAttack += add;
    }

    /// <summary>
    /// ���ʽ� ���� �ӵ� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusAttackSpeed(int add)
    {
        _bonusAttackSpeed += add;
    }

    /// <summary>
    /// ���ʽ� �ִ�ü�� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusMaxHP(int add)
    {
        _bonusMaxHp += add;
        _hp = _maxHp + _bonusMaxHp;
    }
    /// <summary>
    /// ���ʽ� �ִ�ü�� �ۼ�Ʈ ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusMaxHPPercent(int add)
    {
        _bonusMaxHp = _maxHp / 100 * add;
        _hp = _maxHp + _bonusMaxHp;
    }

    /// <summary>
    /// ���ʽ� ���� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusWeight(int add)
    {
        _bonusWeight += add;
    }

    /// <summary>
    /// ���ʽ� �����Ÿ� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusRange(float add)
    {
        _bonusRange += add;
    }

    /// <summary>
    /// ���ʽ� �˹� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusKnockback(int add)
    {
        _bonusKnockback += add;
    }

    /// <summary>
    /// ���ʽ� �˹� ����
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusAccuracy(int add)
    {
        _bonusAccuracy += add;
    }

    /// <summary>
    /// ���ݷ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Attack()
    {
        return Mathf.RoundToInt((_unitData._damage + _bonusAttack) * (float)_attackPercent / 100);
    }
    /// <summary>
    /// �̵��ӵ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_MoveSpeed()
    {
        return (_unitData._moveSpeed + _bonusMoveSpeed) * (float)_moveSpeedPercent / 100;
    }
    /// <summary>
    /// ���ݼӵ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_AttackSpeed()
    {
        return (_unitData._attackSpeed + _bonusAttackSpeed) * (float)_attackSpeedPercent / 100;
    }
    /// <summary>
    /// ��Ÿ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_Range()
    {
        return (_unitData._range +_bonusRange) * (float)_rangePercent / 100;
    }
    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Weight()
    {
        return Mathf.RoundToInt((_unitData._weight + _bonusWeight) * (float)_weightPercent / 100);
    }

    /// <summary>
    /// ���Կ� ���� ��� ��ȯ
    /// </summary>
    /// <returns></returns>
    public WeightGrade ReturnWeightGrade()
	{
        int weight = Return_Weight();
	    if(weight <= 40)
		{
            return WeightGrade.TooLight;
		}
        else if (weight <= 80)
        {
            return WeightGrade.Light;
        }
        else if (weight <= 120)
        {
            return WeightGrade.Normal;
        }
        else if (weight <= 160)
        {
            return WeightGrade.Heavy;
        }
        else
        {
            return WeightGrade.TooHeavy;
        }

    }

    /// <summary>
    /// ���߷� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_Accuracy()
    {
        return (_unitData._accuracy + _bonusAccuracy) * (float)_accuracyPercent / 100;
    }
    /// <summary>
    /// �˹� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Knockback()
    {
        return Mathf.RoundToInt((_unitData._knockback + _bonusKnockback) * (float)_knockbackPercent / 100);
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
    /// ������ �޴·� ������ ����
    /// </summary>
    /// <param name="percent"></param>
    public void DecreseDamage(int Decresedamage)
    {
        _damagedDecrese += Decresedamage;
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
