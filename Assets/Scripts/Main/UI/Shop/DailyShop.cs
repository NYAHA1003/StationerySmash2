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
    private List<DailyCardType> unitTypes = new List<DailyCardType>(); 

    private int dailyCardCount = 6; 
    private void Start()
    {
        
    }

    private void Initialized()
    {
        int enumLength = Enum.GetValues(typeof(DailyCardType)).Length; 
        for (int i = 0; i < enumLength; i++)
        {
            unitTypes.Add((DailyCardType)i);
        }
        int index1,index2,temp;
        index1 = UnityEngine.Random.Range()

    }
    /// <summary>
    /// 老老墨靛 积己 
    /// </summary>
    private void CreateDailyItem()
    {
        // 公丰 墨靛 积己 
        DailyItem freeDailyCard = Instantiate(dailyItemPrefab, transform);
        
        // 蜡丰 墨靛 积己
        for (int i = 0; i < dailyCardCount-1; i++)
        {
            DailyItem paidDailyCard = Instantiate(dailyItemPrefab, transform);
            paidDailyCard.DeepCopy();
        }
    }

}
