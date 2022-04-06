using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCarState : AbstractStateManager
{
    public override void Set_State()
    {
        //������Ʈ���� �����Ѵ�
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
        //�״°� ����
    }


    public override void Set_Throw()
    {
        //������ ����
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