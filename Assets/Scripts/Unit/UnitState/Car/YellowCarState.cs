using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowCarState : AbstractStateManager
{
    public override void Set_State()
    {
        //������Ʈ���� �����Ѵ�
        _idleState = new YellowCarIdleState();
        _waitState = new YellowCarWaitState();
        _moveState = new YellowCarMoveState();
        _damagedState = new YellowCarDamagedState();

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

public class YellowCarIdleState : AbstractIdleState
{
}

public class YellowCarWaitState : AbstractWaitState
{
}

public class YellowCarMoveState : IgnoreMoveState
{
}

public class YellowCarDamagedState : AbstractDamagedState
{
    public override void Enter()
    {
        _stateManager.Set_Wait(0.1f);
    }
}