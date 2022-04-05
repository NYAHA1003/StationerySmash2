using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class UnitStateChanger
{
    public IStateManager StateManager => _stateManager;
    public UnitState UnitState => _unitState;
    private UnitState _unitState = null;
    private StageData _stageData = null;
    private IStateManager _stateManager = null;

    public void ProcessState()
    {
        _unitState = _unitState.Process();
    }
    public void SetUnitState()
    {
        _unitState = _stateManager.Return_CurrentUnitState();
    }
    
    public void SetStageData(StageData stageData)
    {
        _stageData = stageData;
    }

    public void StateNull()
    {
        _unitState = null;
    }

    /// <summary>
    /// 스테이트 추가
    /// </summary>
    public void SetStateManager(UnitType unitType, Transform transform, Transform spriteRendererTransform, Unit unit)
    {
        switch (unitType)
        {
            case UnitType.PencilCase:
                _stateManager = PoolManager.GetItem<PencilCaseStateManager>(transform, spriteRendererTransform, unit);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                _stateManager = PoolManager.GetItem<PencilStateManager>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.BallPen:
                //stateManager = PoolManager.GetItem<BallpenStateManager>(transform, _unitSprite.SpriteRenderer.transform, this);
                break;
        }
        _stateManager.SetStageData(_stageData);
    }


    /// <summary>
    /// 스테이트 삭제
    /// </summary>
    public void DeleteState(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.PencilCase:
                PoolManager.AddItem((PencilCaseStateManager)_stateManager);
                break;

            case UnitType.BallPen:
                //PoolManager.AddItem((BallpenStateManager)stateManager);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                PoolManager.AddItem((PencilStateManager)_stateManager);
                break;
        }
    }
}
