using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

/// <summary>
/// 유닛 스테이트변경자 컴포넌트
/// </summary>
public class UnitStateChanger
{
    //프로퍼티
    public IStateManager StateManager => _stateManager;
    public UnitState UnitState => _unitState;

    //참조변수
    private UnitState _unitState = null;
    private StageData _stageData = null;
    private IStateManager _stateManager = null;

    /// <summary>
    /// 유닛 스테이트 로직 수행
    /// </summary>
    public void ProcessState()
    {
        _unitState = _unitState.Process();
    }

    /// <summary>
    /// 유닛 스테이트 설정
    /// </summary>
    public void SetUnitState()
    {
        _unitState = _stateManager.Return_CurrentUnitState();
    }
    /// <summary>
    /// 스테이지 데이터 설정
    /// </summary>
    /// <param name="stageData"></param>
    
    public void SetStageData(StageData stageData)
    {
        _stageData = stageData;
        _stateManager.SetStageData(_stageData);
    }
    /// <summary>
    /// 스테이트 Null 설정
    /// </summary>
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
