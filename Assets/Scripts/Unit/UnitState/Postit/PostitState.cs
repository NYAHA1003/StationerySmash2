using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostitState : AbstractStateManager
{
    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new PostitIdleState();
        _waitState = new PostitWaitState();
        _moveState = new PostitMoveState();
        _attackState = new PostitAttackState();
        _damagedState = new PostitDamagedState();
        _dieState = new PostitDieState();
        _throwState = new PostitThrowState();

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


public class PostitIdleState : AbstractIdleState
{
}

public class PostitWaitState : AbstractWaitState
{
}

public class PostitMoveState : AbstractMoveState
{
}

public class PostitAttackState : SummonAttackState
{
}

public class PostitDamagedState : AbstractDamagedState
{
}

public class PostitDieState : AbstractDieState
{
}

public class PostitThrowState : AbstractThrowState
{

}

