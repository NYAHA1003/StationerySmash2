using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class SharpState : AbstractStateManager
{
    public CardData SharpsimPieceData => _sharpsimPieceData;
    private CardData _sharpsimPieceData = null;
    private UnitDataSO _unitDataSO = null;
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
        SetShartsim();
        myUnit.SetIsNeverDontThrow(false);
    }
    private async void SetShartsim()
    {
        AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
        await handle.Task;
        _unitDataSO = handle.Result;
        _sharpsimPieceData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == UnitType.SharpSim);
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

public class Sharp_Attack_State : SummonAttackState
{
    protected override void Summon()
    {
        //샤프심 소환
        SharpState eraserState = (SharpState)_stateManager;
        _myUnit.BattleManager.CommandUnit.SummonUnit(eraserState.SharpsimPieceData, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);

    }
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