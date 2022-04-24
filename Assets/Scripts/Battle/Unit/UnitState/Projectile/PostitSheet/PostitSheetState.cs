using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostitSheetState : AbstractStateManager
{

    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new PostitSheetIdleState();
        _waitState = new PostitSheetWaitState();
        _moveState = new PostitSheetMoveState();
        _attackState = new PostitSheetAttackState();
        _damagedState = new PostitSheetDamagedState();
        _dieState = new PostitSheetDieState();
        _throwState = new PostitSheetThrowState();

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


public class PostitSheetIdleState : AbstractIdleState
{
}

public class PostitSheetWaitState : AbstractWaitState
{
}

public class PostitSheetMoveState : AbstractMoveState
{
}

public class PostitSheetAttackState : AbstractAttackState
{
}

public class PostitSheetDamagedState : AbstractDamagedState
{
}

public class PostitSheetDieState : AbstractDieState
{
}

public class PostitSheetThrowState : AbstractThrowState
{

}

