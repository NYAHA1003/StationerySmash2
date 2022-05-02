using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using Utill.Data;
using System;

public class DailyShop : MonoBehaviour
{
    [SerializeField]
    private DailyItem dailyItemPrefab;

    private List<DailyItem> dailyItems = new List<DailyItem>();
    private List<DailyCardType> dailyCardTypes = new List<DailyCardType>(); 

    private int dailyCardCount = 6; 
    private void Start()
    {
        Shuffle();
        CreateDailyItem(); 
    }

    private void Shuffle()
    {
        int enumLength = Enum.GetValues(typeof(DailyCardType)).Length; 
        for (int i = 0; i < enumLength; i++)
        {
            dailyCardTypes.Add((DailyCardType)i);
        }
        int index1,index2;
        DailyCardType temp; 
        for(int i = 0; i < 50; i++)
        {
            index1 = Range(0, dailyCardCount);
            index2 = Range(0, dailyCardCount);
            temp = dailyCardTypes[index1];
            dailyCardTypes[index1] = dailyCardTypes[index2];
            dailyCardTypes[index2] = temp;
        }
    }
    /// <summary>
    /// 老老墨靛 积己 
    /// </summary>
    private void CreateDailyItem()
    {
        // 公丰 墨靛 积己 
        DailyItem freeDailyCard = Instantiate(dailyItemPrefab, transform);
        freeDailyCard.SetCardInfo(dailyCardTypes[0]);

        // 蜡丰 墨靛 积己
        for (int i = 0; i < dailyCardCount; i++)
        {
            DailyItem paidDailyCard = Instantiate(dailyItemPrefab, transform);
            paidDailyCard.SetCardInfo(dailyCardTypes[i]);
        }
    }

}
