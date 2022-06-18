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
    private WarrningComponent _warrningComponent; //경고창
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
    public void Purchase(out bool isbuy)
    {
        // 돈 체크 
        if (UserSaveManagerSO.UserSaveData._money >= _price)
        {
            UserSaveManagerSO.AddMoney(-_price);
        }
        else
		{
            _warrningComponent ??= GameObject.FindObjectOfType<WarrningComponent>();
            _warrningComponent.SetWarrning("돈이 부족합니다");
            isbuy = false;
            return;
		}
        //새카드구매
        UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_stationeryType), 1);
        isbuy = true;
    }
}
