using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class NewBadge 
{
    public DailyItemSO _newBadgeInfo;
    public int[] _price = new int[3] { 50000, 75000, 100000 };

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>();
    public Grade _grade;

    public NewBadge(DailyItemSO dailyItemSO, Grade grade)
    {
        _newBadgeInfo = dailyItemSO;
        ResetDailyItemInfos();

        // 뽑힌 등급과 같은 등급의 스티커들 리스트에 추가 
        for (int i = 0; i < _newBadgeInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemIinfo = _newBadgeInfo.dailyItemInfos[i];
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
    public DailyItemInfo ReturnRandomBadge()
    {
        int index = Random.Range(0, currentDailyItemInfos.Count);
        return currentDailyItemInfos[index];
    }
    /// <summary>
    /// 일일상점 스티커조각 아이템 리스트 초기화
    /// </summary>
    public void ResetDailyItemInfos()
    {
        currentDailyItemInfos.Clear();
    }
}
