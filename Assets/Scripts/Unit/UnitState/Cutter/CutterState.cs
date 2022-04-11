using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterState : AbstractStateManager
{
    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new CutterIdleState();
        _waitState = new CutterWaitState();
        _moveState = new CutterMoveState();
        _attackState = new CutterAttackState();
        _damagedState = new CutterDamagedState();
        _dieState = new CutterDieState();
        _throwState = new CutterThrowState();

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

public class CutterIdleState : AbstractIdleState
{
}

public class CutterWaitState : AbstractWaitState
{
}

public class CutterMoveState : AbstractMoveState
{
}

public class CutterAttackState : SeFiceAttackState
{
}

public class CutterDamagedState : AbstractDamagedState
{
}

public class CutterDieState : AbstractDieState
{
}

public class CutterThrowState : AbstractThrowState
{

}
