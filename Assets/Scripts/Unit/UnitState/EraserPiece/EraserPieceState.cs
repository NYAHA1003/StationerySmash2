using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class EraserPieceState : AbstractStateManager
{
    public override void Set_State()
    {
        //스테이트들을 설정한다
        _idleState = new EraserPieceIdleState();
        _waitState = new EraserPieceWaitState();
        _moveState = new EraserPieceMoveState();
        _attackState = new EraserPieceAttackState();
        _damagedState = new EraserPieceDamagedState();
        _dieState = new EraserPieceDieState();
        _throwState = new EraserPieceThrowState();

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

public class EraserPieceIdleState : AbstractIdleState
{
}

public class EraserPieceWaitState : AbstractWaitState
{
}

public class EraserPieceMoveState : AbstractMoveState
{
}

public class EraserPieceAttackState : AbstractAttackState
{
}

public class EraserPieceDamagedState : AbstractDamagedState
{
}

public class EraserPieceDieState : AbstractDieState
{
}

public class EraserPieceThrowState : AbstractThrowState
{

}

