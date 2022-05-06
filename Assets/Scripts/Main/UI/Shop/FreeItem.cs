using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

public class FreeItem 
{
    public DailyItemSO _freeItemInfo;
    public int _minCount = 1;
    public int _maxCount = 50;
    public int _price = 100;

    public DailyFreeItemType _dailyFreeItemType;
    public FreeItem(DailyItemSO dailyItemSO, DailyFreeItemType dailyFreeItemType)
    {
        _freeItemInfo = dailyItemSO;
        _dailyFreeItemType = dailyFreeItemType; 
        if(_dailyFreeItemType == DailyFreeItemType.Gold)
        {
            _minCount = 100;
            _maxCount = 200; 
        }
        else if(_dailyFreeItemType == DailyFreeItemType.Dalgona)
        {
            _minCount = 3;
            _maxCount = 6; 
        }
    }
    public int ReturnRandomCount()
    {
        int totalCount = Random.Range(_minCount, _maxCount + 1);
        if (_dailyFreeItemType == DailyFreeItemType.Gold)
        {
            return totalCount - totalCount % 10; // 일의 자리수 버림 
        }
        else if(_dailyFreeItemType == DailyFreeItemType.Dalgona)
        {
            return totalCount;
        }
        else
        {
            Debug.LogError("_dailyFreeItemType 에러");
            return -1; 
        }
    }
    public DailyItemInfo ReturnItemInfo(DailyFreeItemType index)
    {
        _freeItemInfo.dailyItemInfos[(int)index]._itemCount = ReturnRandomCount(); 
        return _freeItemInfo.dailyItemInfos[(int)index];
    }
}
