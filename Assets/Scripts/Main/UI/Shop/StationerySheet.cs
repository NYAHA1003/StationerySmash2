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
    private WarrningComponent _warrningComponent; //경고창
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
    public void Purchase(out bool isbuy)
    {
        // 돈 체크 
        if(UserSaveManagerSO.UserSaveData._money >= _price * _curCount)
		{
            UserSaveManagerSO.AddMoney(-_price * _curCount);
        }
        else
        {
            _warrningComponent ??= GameObject.FindObjectOfType<WarrningComponent>();
            _warrningComponent.SetWarrning("돈이 부족합니다");
            isbuy = false;
            return;
        }
        //카드조각구매
        UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_stationeryType), _curCount);
        isbuy = true;
    }
}
