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
    public IStateManager StateManager => _stateManager;
    public UnitState UnitState => _unitState;

    //��������
    private UnitState _unitState = null;
    private StageData _stageData = null;
    private IStateManager _stateManager = null;

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
    /// ������Ʈ ����
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
