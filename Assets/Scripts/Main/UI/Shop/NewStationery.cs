using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool; 
public class NewStationery : IPerchase
{
    public DailyItemSO _newStationeryInfo;
    public int _price = 40000;

    public Grade _grade;
    private CardNamingType _stationeryType;
    public NewStationery(DailyItemSO dailyItemSO)
    {
        _newStationeryInfo = dailyItemSO;
    }
    public DailyItemInfo ReturnItemInfo(CardNamingType stationeryType)
    {
        _stationeryType = stationeryType;
        DailyItemInfo itemInfo = _newStationeryInfo.dailyItemInfos.Find((x) => x._unitCardType == stationeryType);
        itemInfo._itemCount = 1;
        itemInfo._dailyItem = this;
        return itemInfo;
    }
    public void Purchase()
    {
        // 돈 체크 
        //UserSaveManagerSO.AddMoney(-_price);
        Debug.Log("새 카드 구매");
        UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_stationeryType), 1);
    }
}
