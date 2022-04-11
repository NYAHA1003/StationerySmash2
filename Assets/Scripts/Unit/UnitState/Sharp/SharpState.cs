using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using DG.Tweening;
public class SharpState : AbstractStateManager
{
    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new Sharp_Idle_State();
        _waitState = new Sharp_Wait_State();
        _moveState = new Sharp_Move_State();
        _attackState = new Sharp_Attack_State();
        _damagedState = new Sharp_Damaged_State();
        _dieState = new Sharp_Die_State();
        _throwState = new Sharp_Throw_State();

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

public class Sharp_Idle_State : AbstractIdleState
{
}

public class Sharp_Wait_State : AbstractWaitState
{
}

public class Sharp_Move_State : AbstractMoveState
{
}

public class Sharp_Attack_State : AbstractAttackState
{
}

public class Sharp_Damaged_State : AbstractDamagedState
{
}

public class Sharp_Die_State : AbstractDieState
{
}

public class Sharp_Throw_State : AbstractThrowState
{

}