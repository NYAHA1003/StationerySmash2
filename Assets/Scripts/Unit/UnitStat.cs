using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 스탯 컴포넌트
/// </summary>
public class UnitStat
{
    //프로퍼티

    public int AttackPercent => _attackPercent; //공격력 퍼센트
    public int MoveSpeedPercent => _moveSpeedPercent;//이동속도 퍼센트
    public int AttackSpeedPercent => _attackSpeedPercent;//공격속도 퍼센트
    public int RangePercent => _rangePercent;//사정거리 퍼센트
    public int AccuracyPercent => _accuracyPercent;//명중률 퍼센트
    public int WeightPercent => _weightPercent;//무게 퍼센트
    public int KnockbackPercent => _knockbackPercent;//넉백 퍼센트
    public int DamagedPercent => _damagedPercent;//넉백 퍼센트
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
    private int _hp = 0;
    private int _maxHp = 0;
    private int _weight = 0;
    private int _grade = 1;
    private float _attackDelay = 0;

    //참조 변수
    private UnitData _unitData = null;

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
        _grade = grade;
        _moveSpeedPercent = 100 * grade;
        _attackSpeedPercent = 100 * grade;
        _attackPercent = 100 * grade;
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
    public int Return_Attack()
    {
        return Mathf.RoundToInt(_unitData.damage * (float)_attackPercent / 100);
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
