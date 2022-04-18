using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class YellowCarState : AbstractStateManager
{
    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new YellowCarIdleState();
        _waitState = new YellowCarWaitState();
        _moveState = new YellowCarMoveState();
        _damagedState = new YellowCarDamagedState();

        Reset_CurrentUnitState(_idleState);

        _abstractUnitStateList.Add(_idleState);
        _abstractUnitStateList.Add(_waitState);
        _abstractUnitStateList.Add(_moveState);
        _abstractUnitStateList.Add(_damagedState);

        SetInStateList();
    }
    public override void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        myUnit.SetIsNeverDontThrow(true);
        _idleState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _waitState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _moveState.ChangeUnit(myTrm, mySprTrm, myUnit);
        _damagedState.ChangeUnit(myTrm, mySprTrm, myUnit);

        _idleState.ResetState();
        _waitState.ResetState();
        _moveState.ResetState();
        _damagedState.ResetState();

        Set_WaitExtraTime(0);
        Reset_CurrentUnitState(_idleState);
    }

    public override void Set_Die()
    {
        //죽는거 없음
    }


    public override void Set_Throw()
    {
        //던지기 무시
    }
    public override void Set_ThrowPos(Vector2 pos)
    {
        //던지기 무시
    }
}

public class YellowCarIdleState : AbstractIdleState
{
}

public class YellowCarWaitState : AbstractWaitState
{
}

public class YellowCarMoveState : IgnoreMoveState
{
    protected override void CheckTargetUnit(Unit targetUnit)
    {
        AtkData atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, AtkType.SlowDown, EffectType.Attack, 1, 20, 20);
        targetUnit.Run_Damaged(atkData);
    }
}

public class YellowCarDamagedState : AbstractDamagedState
{
    public override void Enter()
    {
        _stateManager.Set_Wait(0.1f);
    }
}