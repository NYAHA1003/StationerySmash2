using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;

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
    public virtual void AddStatusEffect(AtkType atkType, params float[] value)
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
    /// �����̻� ����Ʈ���� ����
    /// </summary>
    public void RemoveStateEff(EffState effState)
    {
        _statEffList.Remove(effState);
    }

}
