using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;
using Battle;

public abstract class UnitState
{
    //프로퍼티
    public eState CurState => _curState;
    public eEvent CurEvent => _curEvent;
    public UnitState NextState => _nextState; // 다음 상태
    public Transform MyTrm => _myTrm;
    public Transform MySprTrm => _mySprTrm;
    public Unit MyUnit => _myUnit;
    public IStateManager StateManager => _stateManager;
    public UnitData MyUnitData => _myUnitData;


    //참조형 변수
    protected Transform _myTrm = null;
    protected Transform _mySprTrm = null;
    public UnitState _nextState = null; // 다음 상태
    protected IStateManager _stateManager = null;
    protected UnitData _myUnitData = null;
    protected Unit _myUnit = null;

    //변수
    protected eState _curState = eState.IDLE;
    protected eEvent _curEvent = eEvent.ENTER;
    protected AtkType originAtkType;
    protected float[] originValue;

    public void Change_Trm(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        _myTrm = myTrm;
        _mySprTrm = mySprTrm;
        _myUnit = myUnit;
        _myUnitData = _myUnit.UnitData;
        originValue = _myUnitData.unitablityData;
    }

    public virtual void Enter() { _curEvent = eEvent.UPDATE; }
    public virtual void Update() { _curEvent = eEvent.UPDATE; }
    public virtual void Exit() { _curEvent = eEvent.EXIT; }

    /// <summary>
    /// 로직 실행
    /// </summary>
    /// <returns></returns>
    public virtual UnitState Process()
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
            return _nextState;
        }

        return this;
    }

    public void Set_Event(eEvent eEvent)
    {
        _curEvent = eEvent;
    }

    public void Set_StateChange(IStateManager stateChange)
    {
        this._stateManager = stateChange;
    }

    public void Reset_State()
    {
        _nextState = null;
        _curEvent = eEvent.ENTER;
    }
    public virtual void Run_Damaged(AtkData atkData)
    {
        if (atkData.damageId == -1)
        {
            //무조건 무시해야할 공격
            return;
        }
        if (atkData.damageId == _myUnit.MyDamagedId)
        {
            //똑같은 공격 아이디를 지닌 공격은 무시함
            return;
        }
        this._stateManager.Set_Damaged(atkData);
    }
    public virtual Unit Pull_Unit()
    {
        if (_myUnit._isDontThrow)
            return null;
        if (_curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        _stateManager.Set_Wait(2);
        return _myUnit;
    }
    public virtual Unit Pulling_Unit()
    {
        if (_myUnit._isDontThrow)
        {
            return null;
        }
        if (_curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        return _myUnit;
    }
    public virtual void Throw_Unit(Vector2 pos)
    {
        _stateManager.Set_ThrowPos(pos);
        _stateManager.Set_Throw();
    }

    public virtual void Animation(params float[] value)
    {

    }

    public void Check_Wall()
    {
        if (_stateManager.GetStageData().max_Range <= _myTrm.position.x)
        {
            //왼쪽으로 튕겨져 나옴
            _myTrm.DOKill();
            _myTrm.DOJump(new Vector3(_myTrm.position.x - 0.2f, 0, _myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
            {
                _stateManager.Set_Wait(0.5f);
            }).SetEase(Utill.Parabola.Return_ParabolaCurve());
        }
        if (-_stateManager.GetStageData().max_Range >= _myTrm.position.x)
        {
            //오른쪽으로 튕겨져 나옴
            _myTrm.DOKill();
            _myTrm.DOJump(new Vector3(_myTrm.position.x + 0.2f, 0, _myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
            {
                _stateManager.Set_Wait(0.5f);
            }).SetEase(Utill.Parabola.Return_ParabolaCurve());
        }
    }

}

