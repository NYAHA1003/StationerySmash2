using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCarState : AbstractStateManager
{
    public override void Set_State()
    {
        //������Ʈ���� �����Ѵ�
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
        //�״°� ����
    }


    public override void Set_Throw()
    {
        //������ ����
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