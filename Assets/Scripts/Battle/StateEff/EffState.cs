using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
using Battle.Units;
public abstract class EffState
{
    //프로퍼티
    public eState CurState => _curState; //현재 스테이트의 상태
    public Transform Trm => _trm; //유닛 트랜스폼
    public Transform SprTrm => _sprTrm; //유닛 스프라이트 트랜스폼
    public EffAttackType StatusType => _statusType; //유닛 상태타입
    public eEvent CurEvent => _curEvent; //현재 이벤트 상태

    //참조형 변수
    private eState _curState = eState.NONE;
    private Transform _trm = null;
    private Transform _sprTrm = null;
    private EffAttackType _statusType = EffAttackType.Normal; //어택타입, 이펙트타입
    protected BattleManager _battleManager = null;
    protected Unit _myUnit = null; //유닛
    protected UnitData _myUnitData = null; // 유닛 데이터
    protected UnitStateEff _unitStateEff = null; // 유닛의 상태이상 컴포트
    protected AbstractStateManager _stateManager = null; //스테이트 매니저
    protected IEffect _effectObj = null; // 이펙트에 사용되는 오브젝트
    
    //변수
    protected eEvent _curEvent = eEvent.ENTER;
    protected float[] _valueList = default; // 상태이상의 특수 값들

    /// <summary>
    /// 상태이상을 설정함
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
        //끝나면 상태이상 삭제
        DeleteStatusEffect();
        _curEvent = eEvent.EXIT; 
    }

    /// <summary>
    /// 상태이상 로직
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
    /// 상태이상의 타입 재설정
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public void SetEffType(EffAttackType atkType, params float[] value)
    {
        _statusType = atkType;
        _valueList = value;
    }

    /// <summary>
    /// 상태이상의 특수값들 재설정
    /// </summary>
    /// <param name="value"></param>
    public abstract void SetEffValue(params float[] value);

    /// <summary>
    /// 효과 삭제
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="eff_State"></param>
    public void DeleteStatusEffect()
    {
        DeleteEffectObject();

        //유닛 상태이상 컴포넌트의 상태이상 리스트에서 삭제함
        _unitStateEff.RemoveStateEff(this);

        //상태이상 반납
        PoolManager.AddEffState(this);
    }

    /// <summary>
    /// 이펙트 오브젝트 반납
    /// </summary>
    public void DeleteEffectObject()
    {
        //이펙트 오브젝트가 있다면 이펙트 오브젝트를 반납함
        if (_effectObj != null)
        {
            _effectObj.Delete_Effect();
            _effectObj = null;
        }
    }
}
