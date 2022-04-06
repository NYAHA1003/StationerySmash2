using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCarState : AbstractStateManager
{
    public override void Set_State()
    {
        //스테이트들을 설정한다
        _idleState = new Pencil_Idle_State();
        _waitState = new Pencil_Wait_State();
        _moveState = new Pencil_Move_State();
        _damagedState = new Pencil_Damaged_State();

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

public class RedCarIdleState : AbstractIdleState
{
}

public class RedCarWaitState : AbstractWaitState
{
}

public class RedCarMoveState : IgnoreMoveState
{
}

public class RedCarDamagedState : AbstractDamagedState
{
    public override void Enter()
    {
        _stateManager.Set_Wait(0.1f);
    }
}