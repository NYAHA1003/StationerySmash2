using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class StickerSheet 
{
    public DailyItemSO _stickerSheetInfo;
    public int _minCount = 1;
    public int _maxCount = 10;
    public int[] _price = new int[3] { 1000, 2000, 4000 };

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>(); 

    public Grade _grade;

    public StickerSheet(DailyItemSO dailyItemSO, Grade grade)
    {
        _stickerSheetInfo = dailyItemSO;
        ResetDailyItemInfos(); 
        
        // 뽑힌 등급과 같은 등급의 스티커들 리스트에 추가 
        for (int i = 0; i < _stickerSheetInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemIinfo = _stickerSheetInfo.dailyItemInfos[i]; 
            if (dailyItemIinfo._grade == grade)
            {
                currentDailyItemInfos.Add(dailyItemIinfo);
            }
        }
    }

    /// <summary>
    /// 현재 등급의 스티커 랜덤으로 하나 리턴 
    /// </summary>
    /// <returns></returns>
    public DailyItemInfo ReturnRandomSticker()
    {
        int index =  Random.Range(0, currentDailyItemInfos.Count);
        Debug.Log(currentDailyItemInfos.Count);
        currentDailyItemInfos[index]._itemCount = ReturnRandomCount(); 
        return currentDailyItemInfos[index];
    }
    /// <summary>
    /// 일일상점 스티커조각 아이템 리스트 초기화
    /// </summary>
    public void ResetDailyItemInfos()
    {
        currentDailyItemInfos.Clear(); 
    }
    /// <summary>
    /// 랜덤 개수 반환 
    /// </summary>
    /// <returns></returns>
    public int ReturnRandomCount()
    {
        return Random.Range(_minCount, _maxCount + 1);
    }
    /// <summary>
    /// 가격 리턴 
    /// </summary>
    /// <param name="grade"></param>
    /// <returns></returns>
    public int ReturnPrice(Grade grade)
    {
        _grade = grade;
        return _price[(int)_grade];
    }

}
