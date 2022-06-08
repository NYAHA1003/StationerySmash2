using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle;
using Battle.Units;

/// <summary>
/// 유닛 스테이트변경자 컴포넌트
/// </summary>
public class UnitStateChanger
{
    //프로퍼티
    public AbstractStateManager StateManager => _stateManager;
    public AbstractUnitState UnitState => _unitState;

    //참조변수
    private AbstractUnitState _unitState = null;
    private StageData _stageData = null;
    private AbstractStateManager _stateManager = null;

    public void ResetUnitStateChanger(CardData cardData, Transform transform, StageData stageData, UnitSprite unitSprite, Unit unit)
    {
        SetStateManager(cardData._unitType, transform, unitSprite.SpriteRenderer.transform, unit); ;
        SetStageData(stageData);
        SetUnitState();
    }

    /// <summary>
    /// 유닛 스테이트 로직 수행
    /// </summary>
    public void ProcessState()
    {
        if(_unitState != null)
		{
            _unitState = _unitState.Process();
		}
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
                _stateManager = PoolManager.GetUnit<PencilCaseStateManager>(transform, spriteRendererTransform, unit);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
                _stateManager = PoolManager.GetUnit<PencilState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.Eraser:
                _stateManager = PoolManager.GetUnit<EraserState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.MechaPencil:
                _stateManager = PoolManager.GetUnit<SharpState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.Pen:
                _stateManager = PoolManager.GetUnit<PenState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.Scissors:
                _stateManager = PoolManager.GetUnit<ScissorsState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.Glue:
                _stateManager = PoolManager.GetUnit<GlueState>(transform, spriteRendererTransform, unit);
                break;

            //필통 관련
            case UnitType.RedCar:
                _stateManager = PoolManager.GetUnit<RedCarState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.YellowCar:
                _stateManager = PoolManager.GetUnit<YellowCarState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.GreenCar:
                _stateManager = PoolManager.GetUnit<GreenCarState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.SharpSim:
                _stateManager = PoolManager.GetUnit<SharpsimState>(transform, spriteRendererTransform, unit);
                break;
        }
        _stateManager.Set_Idle();
    }


    /// <summary>
    /// 스테이트 삭제
    /// </summary>
    public void DeleteState(UnitType unitType)
    {
        PoolManager.AddUnitState(_stateManager);
    }
}
