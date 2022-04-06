using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class EraserState : AbstractStateManager
{
    public DataBase EraserPieceData => _eraserPieceData;
    private DataBase _eraserPieceData;
    private Sprite _eraserPieceSprite;

    public override void Set_State()
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
        SetEraserPieceData(myUnit);
    }

    private void SetEraserPieceData(Unit myUnit)
    {
        //지우개조각 스프라이트 가져오기
        _eraserPieceSprite = null;

        //지우개 조각 데이터 설정
        _eraserPieceData ??= new DataBase()
        {
            card_Cost = 0,
            card_Name = "지우개 조각",
            cardType = CardType.SummonUnit,
            card_Sprite = _eraserPieceSprite
        };
        _eraserPieceData.unitData = myUnit.UnitData;
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


