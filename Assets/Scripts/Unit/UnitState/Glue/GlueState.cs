using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class GlueState : AbstractStateManager
{

    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new GlueIdleState();
        _waitState = new GlueWaitState();
        _moveState = new GlueMoveState();
        _attackState = new GlueAttackState();
        _damagedState = new GlueDamagedState();
        _dieState = new GlueDieState();
        _throwState = new GlueThrowState();

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


public class GlueIdleState : AbstractIdleState
{
}

public class GlueWaitState : AbstractWaitState
{
}

public class GlueMoveState : AbstractMoveState
{
}

public class GlueAttackState : AbstractAttackState
{
    protected override void SetAttackData(ref AtkData atkData)
    {
        atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, AtkType.SlowDown, originValue);
    }
}

public class GlueDamagedState : AbstractDamagedState
{
}

public class GlueDieState : AbstractDieState
{
}

public class GlueThrowState : AbstractThrowState
{

}

