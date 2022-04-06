using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCarState : AbstractStateManager
{
    public override void Set_State()
    {
        //스테이트들을 설정한다
        _idleState = new GreenCarIdleState();
        _waitState = new GreenCarWaitState();
        _moveState = new GreenCarMoveState();
        _damagedState = new GreenCarDamagedState();

        Reset_CurrentUnitState(_idleState);

        _idleState.SetStateManager(this);
        _waitState.SetStateManager(this);
        _moveState.SetStateManager(this);
        _damagedState.SetStateManager(this);
    }

    public override void Set_Die()
    {
        //죽는거 없음
    }


    public override void Set_Throw()
    {
        //던지기 무시
    }
}

public class GreenCarIdleState : AbstractIdleState
{
}

public class GreenCarWaitState : AbstractWaitState
{
}

public class GreenCarMoveState : IgnoreMoveState
{
}

public class GreenCarDamagedState : AbstractDamagedState
{
    public override void Enter()
    {
        _stateManager.Set_Wait(0.1f);
    }
}