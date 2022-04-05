using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilCaseStateManager : IStateManager
{
    private PencilCase_Idle_State IdleState;
    private PencilCase_Damaged_State DamagedState;
    private PencilCase_Die_State DieState;
    private UnitState cur_unitState;
    private StageData _stageData;

    public void Reset_CurrentUnitState(UnitState unitState)
    {
        cur_unitState = unitState;
    }
    public UnitState Return_CurrentUnitState()
    {
        return cur_unitState;
    }

    public void Set_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        //스테이트들을 설정한다
        IdleState = new PencilCase_Idle_State();
        DamagedState = new PencilCase_Damaged_State();
        DieState = new PencilCase_Die_State();

        Reset_CurrentUnitState(IdleState);

        IdleState.Set_StateChange(this);
        DamagedState.Set_StateChange(this);
        DieState.Set_StateChange(this);
    }

    public void SetStageData(StageData stageData)
    {
        _stageData = stageData;
    }

    public void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        IdleState.Change_Trm(myTrm, mySprTrm, myUnit);
        DamagedState.Change_Trm(myTrm, mySprTrm, myUnit);
        DieState.Change_Trm(myTrm, mySprTrm, myUnit);

        IdleState.Reset_State();
        DamagedState.Reset_State();
        DieState.Reset_State();

        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Attack(Unit targetUnit)
    {
        throw new System.Exception("필통 에러");
    }

    public void Set_Damaged(AtkData atkData)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        DamagedState.Set_AtkData(atkData);
        cur_unitState._nextState = DamagedState;
        DamagedState.Reset_State();
        Reset_CurrentUnitState(DamagedState);
    }

    public void Set_Die()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState._nextState = DieState;
        DieState.Reset_State();
        Reset_CurrentUnitState(DieState);
    }

    public void Set_Idle()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState._nextState = IdleState;
        IdleState.Reset_State();
        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Move()
    {
        throw new System.Exception("필통 에러");
    }

    public void Set_Throw()
    {
        throw new System.Exception("필통 에러");
    }

    public void Set_Wait(float time)
    {
        throw new System.Exception("필통 에러");
    }


    public void Set_WaitExtraTime(float extraTime)
    {
        throw new System.Exception("필통 에러");
    }

    public void Set_ThrowPos(Vector2 pos)
    {
        throw new System.Exception("필통 에러");
    }

    public StageData GetStageData()
    {
        return _stageData;
    }
}
public class PencilCase_Idle_State : UnitState
{
    public override Unit Pull_Unit()
    {
        return null;
    }

    public override Unit Pulling_Unit()
    {
        return null;
    }
    public override void Throw_Unit(Vector2 pos)
    {

    }
}
public class PencilCase_Damaged_State : UnitState
{
    private AtkData atkData;

    public void Set_AtkData(AtkData atkData)
    {
        this.atkData = atkData;
    }

    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        _myUnit.SubtractHP(atkData.damage);
        if (_myUnit.UnitStat.Hp <= 0)
        {
            _stateManager.Set_Die();
            return;
        }
        base.Enter();
    }

    public override void Update()
    {
        _stateManager.Set_Idle();
    }

    public override void Animation(params float[] value)
    {
        float rotate = _myUnit.ETeam == TeamType.MyTeam ? 360 : -360;
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), value[0], RotateMode.FastBeyond360);
    }
    public override Unit Pull_Unit()
    {
        return null;
    }

    public override Unit Pulling_Unit()
    {
        return null;
    }
    public override void Throw_Unit(Vector2 pos)
    {

    }
}
public class PencilCase_Die_State : UnitState
{
    public override void Enter()
    {
        //battleManager.CommandCamera.WinCamEffect(myTrm.position, myUnit.eTeam != TeamType.MyTeam);
        base.Enter();
    }

    private void Reset_SprTrm()
    {
        _mySprTrm.localPosition = Vector3.zero;
        _mySprTrm.localScale = Vector3.one;
        _mySprTrm.eulerAngles = Vector3.zero;
        _mySprTrm.DOKill();
    }
    public override Unit Pull_Unit()
    {
        return null;
    }

    public override Unit Pulling_Unit()
    {
        return null;
    }
    public override void Throw_Unit(Vector2 pos)
    {
        
    }
}

