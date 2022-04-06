using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class TrafficAbility : AbstractPencilCaseAbility
{
    private enum TrafficType
    {
        Red,
        Yellow,
        Green,
    }

    private TrafficType _trafficType = TrafficType.Red;
    private DataBase _redCarData = null;
    private DataBase _yellowCarData = null;
    private DataBase _greenCarData = null;
    private UnitDataSO _unitDataSO = null;

    public override void SetState(BattleManager battleManager)
    {
        _battleManager = battleManager;
        SetDatas();
    }

    /// <summary>
    /// ���� ���������Ʈ �����ϼ�, ���� ��� �ʷ� �� ��ȯ
    /// </summary>
    public override void RunPencilCaseAbility()
    {
        switch (_trafficType)
        {
            case TrafficType.Red:
                //������ ��ȯ
                _battleManager.CommandUnit.SummonUnit(_redCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Yellow:
                //����� ����
                _battleManager.CommandUnit.SummonUnit(_yellowCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Green:
                _battleManager.CommandUnit.SummonUnit(_greenCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                //�ʷ��� ��ȯ
                break;
        }

        //���� ��ȣ������ �Ѿ
        _trafficType++;
        if (_trafficType > TrafficType.Green)
        {
            _trafficType = TrafficType.Red;
        }
    }


    /// <summary>
    /// �ڵ��� �����͵� ����
    /// </summary>
    private async void SetDatas()
    {
        AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
        await handle.Task;
        _unitDataSO = handle.Result;
        SetRedCar();
        SetYellowCar();
        SetGreenCar();
    }



    /// <summary>
    /// ������ ������ ����
    /// </summary>
    private void SetRedCar()
    {
        _redCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.RedCar);
    }

    /// <summary>
    /// ����� ������ ����
    /// </summary>
    private void SetYellowCar()
    {
        _yellowCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.YellowCar);
    }

    /// <summary>
    /// �ʷ��� ������ ����
    /// </summary>
    private void SetGreenCar()
    {
        _greenCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.GreenCar);
    }

}
