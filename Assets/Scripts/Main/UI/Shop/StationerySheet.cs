using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

public class StationerySheet 
{
    public DailyItemSO _stationerySheetInfo; 
    public int _minCount = 1;
    public int _maxCount = 50;
    public int _price = 100; 

    public Grade _grade;     

    public StationerySheet(DailyItemSO dailyItemSO)
    {
        _stationerySheetInfo = dailyItemSO;
    }
    public int ReturnRandomCount()
    {
        return Random.Range(_minCount, _maxCount+1); 
    }
    public DailyItemInfo ReturnItemInfo(StationeryType index)
    {
        // 아이템 개수 할당후 반환 
        _stationerySheetInfo.dailyItemInfos[(int)index]._itemCount = ReturnRandomCount(); 
        return _stationerySheetInfo.dailyItemInfos[(int)index];
    }
}
