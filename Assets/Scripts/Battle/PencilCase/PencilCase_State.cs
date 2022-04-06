using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilCaseStateManager : AbstractStateManager
{
    public override void Set_State()
    {
        _idleState = new PencilCaseIdleState();
        _damagedState = new PencilCaseDamagedState();
        _dieState = new PencilCaseDieState();

        Reset_CurrentUnitState(_idleState);

        _idleState.Set_StateChange(this);
        _damagedState.Set_StateChange(this);
        _dieState.Set_StateChange(this);
    }

    public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        _idleState.Change_Trm(myTrm, mySprTrm, myUnit);
        _damagedState.Change_Trm(myTrm, mySprTrm, myUnit);
        _dieState.Change_Trm(myTrm, mySprTrm, myUnit);

        _idleState.Reset_State();
        _damagedState.Reset_State();
        _dieState.Reset_State();

        Set_WaitExtraTime(0);
        Reset_CurrentUnitState(_idleState);
    }
}
public class PencilCaseIdleState : AbstractIdleState
{
    public override void Enter()
    {
        _curState = eState.IDLE;
        _curEvent = eEvent.ENTER;
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
public class PencilCaseDamagedState : AbstractDamagedState
{
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
        _curEvent = eEvent.UPDATE;
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
public class PencilCaseDieState : AbstractDieState
{
    public override void Enter()
    {
        //battleManager.CommandCamera.WinCamEffect(myTrm.position, myUnit.eTeam != TeamType.MyTeam);
        base.Enter();
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

