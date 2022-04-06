using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilState : AbstractStateManager
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

        _idleState.SetStateManager(this);
        _waitState.SetStateManager(this);
        _moveState.SetStateManager(this);
        _attackState.SetStateManager(this);
        _damagedState.SetStateManager(this);
        _dieState.SetStateManager(this);
        _throwState.SetStateManager(this);
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

