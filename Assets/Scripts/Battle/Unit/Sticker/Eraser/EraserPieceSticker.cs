using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class EraserPieceSticker : AbstractSticker
{
    private BattleManager _battleManager;
    private CardData _eraserPieceData = null;
    private UnitDataSO _unitDataSO = null;
    public override void SetSticker(Unit unit)
    {
        base.SetSticker(unit);
        _battleManager = unit.BattleManager;
        SetEarserPieceData();
    }

    public override void RunStickerAblity()
    {
        _myUnit.BattleManager.CommandUnit.SummonUnit(_eraserPieceData, _myUnit.transform.position, _myUnit.UnitStat.Grade, _myUnit.ETeam);
        _myUnit.UnitStat.SetBonusMaxHP(100);
    }
    
    /// <summary>
    /// 지우개 조각 데이터 설정
    /// </summary>
    private async void SetEarserPieceData()
    {
        AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
        await handle.Task;
        _unitDataSO = handle.Result;
        _eraserPieceData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == UnitType.EraserPiece);
    }

}
