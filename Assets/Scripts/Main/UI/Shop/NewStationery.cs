using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class NewStationery 
{
    public DailyItemSO _newStationeryInfo;
    public int _price = 40000;

    public Grade _grade;

    public NewStationery(DailyItemSO dailyItemSO)
    {
        _newStationeryInfo = dailyItemSO;
    }
    public DailyItemInfo ReturnItemInfo(StationeryType index)
    {
        return _newStationeryInfo.dailyItemInfos[(int)index];
    }
}
