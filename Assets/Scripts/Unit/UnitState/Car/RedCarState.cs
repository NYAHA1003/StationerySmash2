using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class RedCarState : AbstractStateManager
{

    public override void Set_State()
    {
        //스테이트들을 설정한다
        _idleState = new RedCarIdleState();
        _waitState = new RedCarWaitState();
        _moveState = new RedCarMoveState();
        _damagedState = new RedCarDamagedState();

        Reset_CurrentUnitState(_idleState);

        _idleState.SetStateManager(this);
        _waitState.SetStateManager(this);
        _moveState.SetStateManager(this);
        _damagedState.SetStateManager(this);
    }
    public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        myUnit.SetIsNeverDontThrow(true);
        _idleState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _waitState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _moveState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _damagedState.ChangeUnit(myTrm, mySprTrm, myUnit);

        _idleState.ResetState();
        _waitState.ResetState();
        _moveState.ResetState();
        _damagedState.ResetState();

        Set_WaitExtraTime(0);
        Reset_CurrentUnitState(_idleState);
    }

    public override void Set_Die()
    {
        //죽는거 없음
    }


    public override void Set_Throw()
    {
        //던지기 무시
    }

    public override void Set_ThrowPos(Vector2 pos)
    {
        //던지기 무시
    }
}

public class RedCarIdleState : AbstractIdleState
{
}

public class RedCarWaitState : AbstractWaitState
{
}

public class RedCarMoveState : IgnoreMoveState
{
    protected override void CheckTargetUnit(Unit targetUnit)
    {
        AtkData atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Damage(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, originAtkType, originValue);
        targetUnit.Run_Damaged(atkData);
    }
}

public class RedCarDamagedState : AbstractDamagedState
{
    public override void Enter()
    {
        _stateManager.Set_Wait(0.1f);
    }
}