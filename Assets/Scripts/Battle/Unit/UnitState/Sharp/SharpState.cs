using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
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

        _abstractUnitStateList.Add(_idleState);
        _abstractUnitStateList.Add(_waitState);
        _abstractUnitStateList.Add(_moveState);
        _abstractUnitStateList.Add(_attackState);
        _abstractUnitStateList.Add(_damagedState);
        _abstractUnitStateList.Add(_dieState);
        _abstractUnitStateList.Add(_throwState);

        SetInStateList();
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