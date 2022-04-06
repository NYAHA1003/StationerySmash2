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

        _idleState.SetStateManager(this);
        _damagedState.SetStateManager(this);
        _dieState.SetStateManager(this);
    }

    public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        _idleState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _damagedState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _dieState.ChangeUnit(myTrm, mySprTrm, myUnit);

        _idleState.ResetState();
        _damagedState.ResetState();
        _dieState.ResetState();

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

    public override Unit PullUnit()
    {
        return null;
    }

    public override Unit PullingUnit()
    {
        return null;
    }
    public override void ThrowUnit(Vector2 pos)
    {

    }
}
public class PencilCaseDamagedState : AbstractDamagedState
{
    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        _myUnit.SubtractHP(_atkData.damage);
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
    public override Unit PullUnit()
    {
        return null;
    }

    public override Unit PullingUnit()
    {
        return null;
    }
    public override void ThrowUnit(Vector2 pos)
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
    public override Unit PullUnit()
    {
        return null;
    }

    public override Unit PullingUnit()
    {
        return null;
    }
    public override void ThrowUnit(Vector2 pos)
    {
        
    }
}

