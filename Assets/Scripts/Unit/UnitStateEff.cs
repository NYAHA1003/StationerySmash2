using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;

/// <summary>
/// 유닛 상태이상 컴포넌트
/// </summary>
public class UnitStateEff
{
    //변수
    private List<EffState> _statEffList = new List<EffState>();
    
    //참조 변수
    private Unit _unit = null;
    private Transform _transform = null;
    private SpriteRenderer _spriteRenderer = null;



    /// <summary>
    /// 상태이상 컴포넌트 설정
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="spriteRenderer"></param>
    public void SetStateEff(Unit unit, SpriteRenderer spriteRenderer)
    {
        _unit = unit;
        _transform = unit.transform;
        _spriteRenderer = spriteRenderer;
    }

    /// <summary>
    /// 상태이상 수행
    /// </summary>
    public void ProcessEff()
    {
        for (int i = 0; i < _statEffList.Count; i++)
        {
            _statEffList[i].Process();
        }
    }

    /// <summary>
    /// 모든 상태이상 삭제
    /// </summary>
    public void DeleteEffStetes()
    {
        //모든 상태이상 삭제
        for (; _statEffList.Count > 0;)
        {
            _statEffList[0].DeleteStatusEffect();
        }
    }

    /// <summary>
    /// 효과 추가
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddStatusEffect(AtkType atkType, params float[] value)
    {
        //이미 있는 효과인지 찾기
        EffState statEffState = _statEffList.Find(x => x.StatusType.Equals(atkType));
        if (statEffState != null)
        {
            statEffState.SetEffValue(value);
            return;
        }

        //기존 효과가 없으면 추가
        switch (atkType)
        {
            case AtkType.Normal:
                return;
            case AtkType.Stun:
                _statEffList.Add(PoolManager.GetEff<StunEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                return;
            case AtkType.Ink:
                _statEffList.Add(PoolManager.GetEff<InkEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                return;
            case AtkType.SlowDown:
                _statEffList.Add(PoolManager.GetEff<SlowEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                return;
            case AtkType.Rage:
                _statEffList.Add(PoolManager.GetEff<RageEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case AtkType.Rtac:
                _statEffList.Add(PoolManager.GetEff<RtacEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case AtkType.Blind:
                _statEffList.Add(PoolManager.GetEff<BlindEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case AtkType.Sick:
                _statEffList.Add(PoolManager.GetEff<SickEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case AtkType.Exch:
                _statEffList.Add(PoolManager.GetEff<FastfEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
        }
    }

    /// <summary>
    /// 상태이상 리스트에서 제거
    /// </summary>
    public void RemoveStateEff(EffState effState)
    {
        _statEffList.Remove(effState);
    }

}
