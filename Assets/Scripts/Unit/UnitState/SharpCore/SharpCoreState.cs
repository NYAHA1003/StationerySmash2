using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpCoreState : AbstractStateManager
{
    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new SharpCoreIdleState();
        _waitState = new SharpCoreWaitState();
        _moveState = new SharpCoreMoveState();
        _attackState = new SharpCoreAttackState();
        _damagedState = new SharpCoreDamagedState();
        _dieState = new SharpCoreDieState();
        _throwState = new SharpCoreThrowState();

        Reset_CurrentUnitState(_idleState);

        _idleState.SetStateManager(this);
        _waitState.SetStateManager(this);
        _moveState.SetStateManager(this);
        _attackState.SetStateManager(this);
        _damagedState.SetStateManager(this);
        _dieState.SetStateManager(this);
        _throwState.SetStateManager(this);
    }

    public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        base.Reset_State(myTrm, mySprTrm, myUnit);
        myUnit.SetIsNeverDontThrow(false);
    }
}

public class SharpCoreIdleState : AbstractIdleState
{
}

public class SharpCoreWaitState : AbstractWaitState
{
}

public class SharpCoreMoveState : AbstractMoveState
{
}

public class SharpCoreAttackState : CritAttackState
{
}

public class SharpCoreDamagedState : AbstractDamagedState
{
}

public class SharpCoreDieState : AbstractDieState
{
}

public class SharpCoreThrowState : AbstractThrowState
{

}

