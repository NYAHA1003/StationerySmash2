using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

public abstract class AbstractDailyItem : MonoBehaviour
{
    public DailyItemSO _newItemInfo;
    public abstract int[] _price { get; set; }

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>();
    public Grade _grade;
    
    public AbstractDailyItem(DailyItemSO dailyItemSO, Grade grade = Grade.Null) 
    {
        _newItemInfo = dailyItemSO;
        ResetDailyItemInfos();

        if (grade == Grade.Null) return; 

        // 뽑힌 등급과 같은 등급의 아이템들 리스트에 추가 
        for(int i = 0; i< _newItemInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemInfo = _newItemInfo.dailyItemInfos[i]; 
            if(dailyItemInfo._grade == grade)
            {
                currentDailyItemInfos.Add(dailyItemInfo); 
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
