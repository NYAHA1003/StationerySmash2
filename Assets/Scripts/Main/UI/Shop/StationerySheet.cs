using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool; 
public class StationerySheet : IPerchase
{
    public DailyItemSO _stationerySheetInfo; 
    public int _minCount = 1;
    public int _maxCount = 50;
    public int _price = 100; 

    public Grade _grade;
    private CardNamingType _stationeryType;
    private int _curCount; 
    public StationerySheet(DailyItemSO dailyItemSO)
    {
        _stationerySheetInfo = dailyItemSO;
    }
    public int ReturnRandomCount()
    {
        _curCount = Random.Range(_minCount, _maxCount + 1);
        return _curCount; 
    }
    public DailyItemInfo ReturnItemInfo(CardNamingType stationeryType)
    {
        // 아이템 개수 할당후 반환 
        _stationeryType = stationeryType;
        DailyItemInfo itemInfo = _stationerySheetInfo.dailyItemInfos.Find((x) => x._unitCardType == stationeryType);

        itemInfo._itemCount = ReturnRandomCount();
        itemInfo._dailyItem = this; 
        return itemInfo;
    }
    public void Purchase()
    {
        // 돈 체크 
        //UserSaveManagerSO.AddMoney(-_price * _curCount);
        Debug.Log("카드 조각 구매");
        UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_stationeryType), _curCount);
        
    }
}
