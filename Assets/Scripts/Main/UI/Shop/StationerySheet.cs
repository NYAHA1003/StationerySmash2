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
    private WarrningComponent _warrningComponent; //���â
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
        // ������ ���� �Ҵ��� ��ȯ 
        _stationeryType = stationeryType;
        DailyItemInfo itemInfo = _stationerySheetInfo.dailyItemInfos.Find((x) => x._unitCardType == stationeryType);

        itemInfo._itemCount = ReturnRandomCount();
        itemInfo._dailyItem = this; 
        return itemInfo;
    }
    public void Purchase(out bool isbuy)
    {
        // �� üũ 
        if(UserSaveManagerSO.UserSaveData._money >= _price * _curCount)
		{
            UserSaveManagerSO.AddMoney(-_price * _curCount);
        }
        else
        {
            _warrningComponent ??= GameObject.FindObjectOfType<WarrningComponent>();
            _warrningComponent.SetWarrning("���� �����մϴ�");
            isbuy = false;
            return;
        }
        //ī����������
        UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_stationeryType), _curCount);
        isbuy = true;
    }
}
