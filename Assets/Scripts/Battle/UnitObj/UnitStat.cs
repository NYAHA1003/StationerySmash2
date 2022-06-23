using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 스탯 컴포넌트
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

    //프로퍼티
    public int AttackPercent => _attackPercent; //공격력 퍼센트
    public int MoveSpeedPercent => _moveSpeedPercent;//이동속도 퍼센트
    public int AttackSpeedPercent => _attackSpeedPercent;//공격속도 퍼센트
    public int RangePercent => _rangePercent;//사정거리 퍼센트
    public int AccuracyPercent => _accuracyPercent;//명중률 퍼센트
    public int WeightPercent => _weightPercent;//무게 퍼센트
    public int KnockbackPercent => _knockbackPercent;//넉백 퍼센트
    public int DamagedPercent => _damagedPercent;// 데미지 퍼센트
    public int DamageDecrese => _damagedDecrese; // 데미지
    public int Hp => _hp;//체력 퍼센트
    public int MaxHp => _maxHp;//최대체력 퍼센트
    public int Grade => _grade; //현재 단계
    public float AttackDelay => _attackDelay;//공격 딜레이

    //변수
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

    //참조 변수
    private UnitData _unitData = null;

    /// <summary>
    /// 초기화
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
    /// 무게 설정
    /// </summary>
    public void SetWeight()
    {
        _weight = _unitData._weight;
    }
    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public void SubtractHP(int damage) //내가 바꿔놓음
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
    /// 보너스 스탯 초기화
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
    /// 잃은 체력의 일정 퍼센트 돌려주기
    /// </summary>
    /// <param name="return"></param>
    public int LostHpPenrcent(int percent)
    {
        int lostHPpercent;
        lostHPpercent = (1 - _hp / _maxHp) * percent;
        return lostHPpercent;
    }

    /// <summary>
    /// 보너스 이동 속도 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusMoveSpeed(int add)
    {
        _bonusMoveSpeed += add;
    }

    /// <summary>
    /// 보너스 공격력 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusAttack(int add)
    {
        _bonusAttack += add;
    }

    /// <summary>
    /// 보너스 공격 속도 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusAttackSpeed(int add)
    {
        _bonusAttackSpeed += add;
    }

    /// <summary>
    /// 보너스 최대체력 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusMaxHP(int add)
    {
        _bonusMaxHp += add;
        _hp = _maxHp + _bonusMaxHp;
    }
    /// <summary>
    /// 보너스 최대체력 퍼센트 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusMaxHPPercent(int add)
    {
        _bonusMaxHp = _maxHp / 100 * add;
        _hp = _maxHp + _bonusMaxHp;
    }

    /// <summary>
    /// 보너스 무게 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusWeight(int add)
    {
        _bonusWeight += add;
    }

    /// <summary>
    /// 보너스 사정거리 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusRange(float add)
    {
        _bonusRange += add;
    }

    /// <summary>
    /// 보너스 넉백 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusKnockback(int add)
    {
        _bonusKnockback += add;
    }

    /// <summary>
    /// 보너스 넉백 설정
    /// </summary>
    /// <param name="add"></param>
    public void SetBonusAccuracy(int add)
    {
        _bonusAccuracy += add;
    }

    /// <summary>
    /// 공격력 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Attack()
    {
        return Mathf.RoundToInt((_unitData._damage + _bonusAttack) * (float)_attackPercent / 100);
    }
    /// <summary>
    /// 이동속도 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_MoveSpeed()
    {
        return (_unitData._moveSpeed + _bonusMoveSpeed) * (float)_moveSpeedPercent / 100;
    }
    /// <summary>
    /// 공격속도 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_AttackSpeed()
    {
        return (_unitData._attackSpeed + _bonusAttackSpeed) * (float)_attackSpeedPercent / 100;
    }
    /// <summary>
    /// 사거리 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_Range()
    {
        return (_unitData._range +_bonusRange) * (float)_rangePercent / 100;
    }
    /// <summary>
    /// 무게 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Weight()
    {
        return Mathf.RoundToInt((_unitData._weight + _bonusWeight) * (float)_weightPercent / 100);
    }

    /// <summary>
    /// 무게에 따른 등급 반환
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
    /// 명중률 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_Accuracy()
    {
        return (_unitData._accuracy + _bonusAccuracy) * (float)_accuracyPercent / 100;
    }
    /// <summary>
    /// 넉백 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Knockback()
    {
        return Mathf.RoundToInt((_unitData._knockback + _bonusKnockback) * (float)_knockbackPercent / 100);
    }
    /// <summary>
    /// 공격력 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseAttackPercent(int percent)
    {
        _attackPercent += percent;
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
    /// <summary>
    /// 데미지 받는량 퍼센트 조절
    /// </summary>
    /// <param name="percent"></param>
    public void IncreaseDamagedPercent(int percent)
    {
        _damagedPercent += percent;
    }
    /// <summary>
    /// 데미지 받는량 깡으로 조절
    /// </summary>
    /// <param name="percent"></param>
    public void DecreseDamage(int Decresedamage)
    {
        _damagedDecrese += Decresedamage;
    }
    /// <summary>
    /// 공격 딜레이 0으로 초기화
    /// </summary>
    public void ResetAttackDelay()
    {
        _attackDelay = 0;
    }

    /// <summary>
    /// 공격 딜레이 설정
    /// </summary>
    /// <param name="delay"></param>
    public void SetAttackDelay(float delay)
    {
        _attackDelay = delay;
    }

}
