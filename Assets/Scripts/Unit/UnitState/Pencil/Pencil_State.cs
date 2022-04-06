using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilStateManager : AbstractStateManager
{
    public override void Set_State()
    {
        //스테이트들을 설정한다
        _idleState = new Pencil_Idle_State();
        _waitState = new Pencil_Wait_State();
        _moveState = new Pencil_Move_State();
        _attackState = new Pencil_Attack_State();
        _damagedState = new Pencil_Damaged_State();
        _dieState = new Pencil_Die_State();
        _throwState = new Pencil_Throw_State();

        Reset_CurrentUnitState(_idleState);

        _idleState.Set_StateChange(this);
        _waitState.Set_StateChange(this);
        _moveState.Set_StateChange(this);
        _attackState.Set_StateChange(this);
        _damagedState.Set_StateChange(this);
        _dieState.Set_StateChange(this);
        _throwState.Set_StateChange(this);
    }
}

public class Pencil_Idle_State : AbstractIdleState
{
}

public class Pencil_Wait_State : AbstractWaitState
{
}

public class Pencil_Move_State : AbstractMoveState
{
}

public class Pencil_Attack_State : AbstractAttackState
{
}

public class Pencil_Damaged_State : AbstractDamagedState
{
}

public class Pencil_Die_State : AbstractDieState
{
}

public class Pencil_Throw_State : AbstractThrowState
{

}

