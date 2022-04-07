using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

/// <summary>
/// ���� ������Ʈ������ ������Ʈ
/// </summary>
public class UnitStateChanger
{
    //������Ƽ
    public AbstractStateManager StateManager => _stateManager;
    public AbstractUnitState UnitState => _unitState;

    //��������
    private AbstractUnitState _unitState = null;
    private StageData _stageData = null;
    private AbstractStateManager _stateManager = null;

    /// <summary>
    /// ���� ������Ʈ ���� ����
    /// </summary>
    public void ProcessState()
    {
        _unitState = _unitState.Process();
    }

    /// <summary>
    /// ���� ������Ʈ ����
    /// </summary>
    public void SetUnitState()
    {
        _unitState = _stateManager.Return_CurrentUnitState();
    }
    /// <summary>
    /// �������� ������ ����
    /// </summary>
    /// <param name="stageData"></param>
    
    public void SetStageData(StageData stageData)
    {
        _stageData = stageData;
        _stateManager.SetStageData(_stageData);
    }
    /// <summary>
    /// ������Ʈ Null ����
    /// </summary>
    public void StateNull()
    {
        _unitState = null;
    }

    /// <summary>
    /// ������Ʈ �߰�
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

                //���� ����
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
    /// ������Ʈ ����
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

        //    //������Ÿ������
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
