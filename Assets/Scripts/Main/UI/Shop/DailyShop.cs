using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Utill.Data;
using System;

public class DailyShop : MonoBehaviour
{
    [SerializeField]
    private DailyItem dailyItemPrefab;

    private List<DailyItem> dailyItems = new List<DailyItem>();
    private List<ItemType> itemTypes = new List<ItemType>(); // 일일상점 아이템타입
    private List<DailyCardType> dailyCardTypes = new List<DailyCardType>(); // 일일상점 카드 타입

    // 임시 카드 보유 여부
    private bool _Pencil = true;
    private bool _MechaPencil = true;
    private bool _Eraser = true;
    private bool _Scissors = false;
    private bool _Glue = false;
    private bool _Ruler = false;
    private bool _Cutterknife = false;
    private bool _Postit = false;
    private bool _MechaPencilLead = false;
    private bool _Pen = false;
    // 끝

    private int dailyCardCount = 6; 
    private void Start()
    {
        Shuffle();
        CreateDailyItem(); 
    }

    private void Shuffle()
    {
        int enumLength = Enum.GetValues(typeof(DailyCardType)).Length; 
        for (int i = 0; i < 600; i++)
        {
            //dailyCardTypes.Add(DailyCardType.);
        }
        int index1,index2;
        DailyCardType temp; 
        for(int i = 0; i < 50; i++)
        {
            index1 = Random.Range(0, dailyCardCount);
            index2 = Random.Range(0, dailyCardCount);
            temp = dailyCardTypes[index1];
            dailyCardTypes[index1] = dailyCardTypes[index2];
            dailyCardTypes[index2] = temp;
        }
    }
    /// <summary>
    /// 일일카드 생성 
    /// </summary>
    private void CreateDailyItem()
    {
        // 무료 카드 생성 
        int freeItemCount; // 무료템 개수
        int freeItemPercent; // 무료템 확률
        DailyFreeItemType dailyFreeItemType; 
        freeItemPercent = Random.Range(0, 10);
        if (freeItemPercent > 3) // 70%확률로 골드 
        {
            dailyFreeItemType = DailyFreeItemType.Gold; 
            freeItemCount = Random.Range(10, 21) * 10;
        }
        else // 30%확률로 달고나 
        {
            dailyFreeItemType = DailyFreeItemType.Dalgona;
            freeItemCount = Random.Range(3, 7);
        }
        DailyItem freeDailyCard = Instantiate(dailyItemPrefab, transform);
        freeDailyCard.SetCardInfo(dailyCardTypes[0], freeItemCount);


        int paidItemCount; // 유료템 개수
        int paidItemPercent; // 유료템 확률
        DailyCardType dailyCardType;

        // 유료 카드 생성
        for (int i = 0; i < dailyCardCount; i++)
        {
            DailyItem paidDailyCard = Instantiate(dailyItemPrefab, transform);
            //paidDailyCard.SetCardInfo(dailyCardTypes[i]);
        }
    }

}
