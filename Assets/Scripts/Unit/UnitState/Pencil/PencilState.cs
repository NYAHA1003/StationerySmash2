using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilState : AbstractStateManager
{
    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new PencilIdleState();
        _waitState = new PencilWaitState();
        _moveState = new PencilMoveState();
        _attackState = new PencilAttackState();
        _damagedState = new PencilDamagedState();
        _dieState = new PencilDieState();
        _throwState = new PencilThrowState();

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

public class PencilIdleState : AbstractIdleState
{
}

public class PencilWaitState : AbstractWaitState
{
}

public class PencilMoveState : AbstractMoveState
{
}

public class PencilAttackState : AbstractAttackState
{
}

public class PencilDamagedState : AbstractDamagedState
{
}

public class PencilDieState : AbstractDieState
{
}

public class PencilThrowState : AbstractThrowState
{

}

