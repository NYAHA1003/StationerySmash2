using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Battle;
using Battle.StateEff;

/// <summary>
/// ���� �����̻� ������Ʈ
/// </summary>
public class UnitStateEff
{
    //����
    private List<EffState> _statEffList = new List<EffState>();
    
    //���� ����
    private Unit _unit = null;
    private Transform _transform = null;
    private SpriteRenderer _spriteRenderer = null;



    /// <summary>
    /// �����̻� ������Ʈ ����
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
    /// �����̻� ����
    /// </summary>
    public void ProcessEff()
    {
        for (int i = 0; i < _statEffList.Count; i++)
        {
            _statEffList[i].Process();
        }
    }

    /// <summary>
    /// ��� �����̻� ����
    /// </summary>
    public void DeleteEffStetes()
    {
        //��� �����̻� ����
        for (; _statEffList.Count > 0;)
        {
            _statEffList[0].DeleteStatusEffect();
        }
    }

    /// <summary>
    /// ȿ�� �߰�
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddStatusEffect(EffAttackType atkType, params float[] value)
    {
        //�̹� �ִ� ȿ������ ã��
        EffState statEffState = _statEffList.Find(x => x.StatusType.Equals(atkType));
        if (statEffState != null)
        {
            statEffState.SetEffValue(value);
            return;
        }

        //���� ȿ���� ������ �߰�
        switch (atkType)
        {
            case EffAttackType.Normal:
                return;
            case EffAttackType.Stun:
                _statEffList.Add(PoolManager.GetEff<StunEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                return;
            case EffAttackType.Ink:
                _statEffList.Add(PoolManager.GetEff<InkEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                return;
            case EffAttackType.SlowDown:
                _statEffList.Add(PoolManager.GetEff<SlowEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                return;
            case EffAttackType.Rage:
                _statEffList.Add(PoolManager.GetEff<RageEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case EffAttackType.Rtac:
                _statEffList.Add(PoolManager.GetEff<RtacEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case EffAttackType.Blind:
                _statEffList.Add(PoolManager.GetEff<BlindEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case EffAttackType.Sick:
                _statEffList.Add(PoolManager.GetEff<SickEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case EffAttackType.Exch:
                _statEffList.Add(PoolManager.GetEff<FastfEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
            case EffAttackType.Scratch:
                _statEffList.Add(PoolManager.GetEff<ScratchEffState>(_transform, _spriteRenderer.transform, _unit, atkType, value));
                break;
        }
    }

    /// <summary>
    /// �����̻� ����Ʈ���� ����
    /// </summary>
    public void RemoveStateEff(EffState effState)
    {
        _statEffList.Remove(effState);
    }

}
