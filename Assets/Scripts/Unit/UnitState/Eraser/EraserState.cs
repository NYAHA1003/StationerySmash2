using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;


public class EraserState : AbstractStateManager
{
    public CardData EraserPieceData => _eraserPieceData;
    private CardData _eraserPieceData = null;
    private UnitDataSO _unitDataSO = null;

    public override void SetState()
    {
        //스테이트들을 설정한다
        _idleState = new EraserIdleState();
        _waitState = new EraserWaitState();
        _moveState = new EraserMoveState();
        _attackState = new EraserAttackState();
        _damagedState = new EraserDamagedState();
        _dieState = new EraserDieState();
        _throwState = new EraserThrowState();

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
        SetEraserPieceData();
        myUnit.SetIsNeverDontThrow(false);
    }

    private async void SetEraserPieceData()
    {
        AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
        await handle.Task;
        _unitDataSO = handle.Result;
        _eraserPieceData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == UnitType.EraserPiece);
    }

}

public class EraserIdleState : AbstractIdleState
{
}

public class EraserWaitState : AbstractWaitState
{
}

public class EraserMoveState : AbstractMoveState
{
}

public class EraserAttackState : AbstractAttackState
{
}

public class EraserDamagedState : AbstractDamagedState
{
}

public class EraserDieState : WillDieState
{
    protected override void Will()
    {
        //지우개 조각 소환
        EraserState eraserState = (EraserState)_stateManager;
        _myUnit.BattleManager.CommandUnit.SummonUnit(eraserState.EraserPieceData, _myTrm.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);

        base.Will();
    }
}

public class EraserThrowState : AbstractThrowState
{

}


