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
    public AbstractStateManager StateManager => _stateManager;
    public AbstractUnitState UnitState => _unitState;

    //참조변수
    private AbstractUnitState _unitState = null;
    private StageData _stageData = null;
    private AbstractStateManager _stateManager = null;

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
            case UnitType.Sharp:
                _stateManager = PoolManager.GetUnit<SharpState>(transform, spriteRendererTransform, unit);
                break;
            case UnitType.BallPen:
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
        }
    }


    /// <summary>
    /// 스테이트 삭제
    /// </summary>
    public void DeleteState(UnitType unitType)
    {
        var type = typeof(PoolManager);
        var method = type.GetMethod("AddItem");
        var gMethod = method.MakeGenericMethod(_stateManager.GetType());
        gMethod.Invoke(null, new object[] { _stateManager });

        //switch (unitType)
        //{
        //    case UnitType.PencilCase:
        //        PoolManager.AddItem((PencilCaseStateManager)_stateManager);
        //        break;

        //    case UnitType.BallPen:
        //        //PoolManager.AddItem((BallpenStateManager)stateManager);
        //        break;
        //    default:
        //    case UnitType.None:
        //    case UnitType.Pencil:
        //    case UnitType.Eraser:
        //    case UnitType.Sharp:
        //        PoolManager.AddItem((PencilState)_stateManager);
        //        break;

        //    //프로젝타일유닛
        //    case UnitType.RedCar:
        //        PoolManager.AddItem((RedCarState)_stateManager);
        //        break;
        //    case UnitType.YellowCar:
        //        PoolManager.AddItem((YellowCarState)_stateManager);
        //        break;
        //    case UnitType.GreenCar:
        //        PoolManager.AddItem((GreenCarState)_stateManager);
        //        break;
        //}
    }
}
