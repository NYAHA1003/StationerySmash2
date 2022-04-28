using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
using Battle.Units;
public abstract class EffState
{
    //������Ƽ
    public eState CurState => _curState; //���� ������Ʈ�� ����
    public Transform Trm => _trm; //���� Ʈ������
    public Transform SprTrm => _sprTrm; //���� ��������Ʈ Ʈ������
    public EffAttackType StatusType => _statusType; //���� ����Ÿ��
    public eEvent CurEvent => _curEvent; //���� �̺�Ʈ ����

    //������ ����
    private eState _curState = eState.NONE;
    private Transform _trm = null;
    private Transform _sprTrm = null;
    private EffAttackType _statusType = EffAttackType.Normal; //����Ÿ��, ����ƮŸ��
    protected BattleManager _battleManager = null;
    protected Unit _myUnit = null; //����
    protected UnitData _myUnitData = null; // ���� ������
    protected UnitStateEff _unitStateEff = null; // ������ �����̻� ����Ʈ
    protected AbstractStateManager _stateManager = null; //������Ʈ �Ŵ���
    protected IEffect _effectObj = null; // ����Ʈ�� ���Ǵ� ������Ʈ
    
    //����
    protected eEvent _curEvent = eEvent.ENTER;
    protected float[] _valueList = default; // �����̻��� Ư�� ����

    /// <summary>
    /// �����̻��� ������
    /// </summary>
    /// <param name="myTrm"></param>
    /// <param name="mySprTrm"></param>
    /// <param name="myUnit"></param>
    /// <param name="statusEffect"></param>
    /// <param name="valueList"></param>
    public void SetStateEff(Transform myTrm, Transform mySprTrm, Unit myUnit, EffAttackType statusEffect, params float[] valueList)
    {
        _curEvent = eEvent.ENTER;
        _trm = myTrm;
        _sprTrm = mySprTrm;
        _myUnit = myUnit;
        _unitStateEff = myUnit.UnitStateEff;
        _stateManager = myUnit.UnitStateChanger.StateManager;

        SetEffType(statusEffect, valueList);
        SetEffValue(valueList);
        this._battleManager = myUnit.BattleManager;
    }

    public virtual void Enter() { _curEvent = eEvent.UPDATE; }
    public virtual void Update() { _curEvent = eEvent.UPDATE; }
    public virtual void Exit() 
    {
        //������ �����̻� ����
        DeleteStatusEffect();
        _curEvent = eEvent.EXIT; 
    }

    /// <summary>
    /// �����̻� ����
    /// </summary>
    public void Process()
    {
        if (_curEvent.Equals(eEvent.ENTER))
        {
            Enter();
        }
        if (_curEvent.Equals(eEvent.UPDATE))
        {
            Update();
        }
        if (_curEvent.Equals(eEvent.EXIT))
        {
            Exit();
        }
    }

    /// <summary>
    /// �����̻��� Ÿ�� �缳��
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public void SetEffType(EffAttackType atkType, params float[] value)
    {
        _statusType = atkType;
        _valueList = value;
    }

    /// <summary>
    /// �����̻��� Ư������ �缳��
    /// </summary>
    /// <param name="value"></param>
    public abstract void SetEffValue(params float[] value);

    /// <summary>
    /// ȿ�� ����
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="eff_State"></param>
    public void DeleteStatusEffect()
    {
        DeleteEffectObject();

        //���� �����̻� ������Ʈ�� �����̻� ����Ʈ���� ������
        _unitStateEff.RemoveStateEff(this);

        //�����̻� �ݳ�
        PoolManager.AddEffState(this);
    }

    /// <summary>
    /// ����Ʈ ������Ʈ �ݳ�
    /// </summary>
    public void DeleteEffectObject()
    {
        //����Ʈ ������Ʈ�� �ִٸ� ����Ʈ ������Ʈ�� �ݳ���
        if (_effectObj != null)
        {
            _effectObj.Delete_Effect();
            _effectObj = null;
        }
    }
}
