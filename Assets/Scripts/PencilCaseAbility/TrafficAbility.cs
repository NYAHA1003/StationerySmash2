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
    /// 구글 스프레드시트 참고하셈, 빨강 노랑 초록 차 소환
    /// </summary>
    public override void RunPencilCaseAbility()
    {
        switch (_trafficType)
        {
            case TrafficType.Red:
                //빨간차 소환
                _battleManager.CommandUnit.SummonUnit(_redCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Yellow:
                //노란차 소한
                _battleManager.CommandUnit.SummonUnit(_yellowCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Green:
                _battleManager.CommandUnit.SummonUnit(_greenCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                //초록차 소환
                break;
        }

        //다음 신호등으로 넘어감
        _trafficType++;
        if (_trafficType > TrafficType.Green)
        {
            _trafficType = TrafficType.Red;
        }
    }


    /// <summary>
    /// 자동차 데이터들 설정
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
    /// 빨간차 데이터 설정
    /// </summary>
    private void SetRedCar()
    {
        _redCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.RedCar);
    }

    /// <summary>
    /// 노란차 데이터 설정
    /// </summary>
    private void SetYellowCar()
    {
        _yellowCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.YellowCar);
    }

    /// <summary>
    /// 초록차 데이터 설정
    /// </summary>
    private void SetGreenCar()
    {
        _greenCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.GreenCar);
    }

}
