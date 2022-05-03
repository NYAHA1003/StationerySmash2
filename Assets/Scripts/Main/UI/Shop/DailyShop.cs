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
        Initialized();
        ShowFreeDailyCard();
        ShowDailyCard();
        //Shuffle();
        //CreateDailyItem();
    }
    /// <summary>
    /// 일일상점 카드타입 확률에따라 넣어주기
    /// 60% : 학용품 조각
    /// 25% : 스티커 조각
    /// 13% : 뱃지 조각
    /// 1% : 신규 학용품
    /// 0.7% : 신규 스티커
    /// 0.3% : 신규 뱃지
    /// </summary>
    private void Initialized()
    {
        for (int i = 0; i < 600; i++)
        {
            dailyCardTypes.Add(DailyCardType.StationerySheet);
        }
        for (int i = 0; i < 250; i++)
        {
            dailyCardTypes.Add(DailyCardType.StickerSheet);
        }
        for (int i = 0; i < 130; i++)
        {
            dailyCardTypes.Add(DailyCardType.BadgeSheet);
        }
        for (int i = 0; i < 10; i++)
        {
            dailyCardTypes.Add(DailyCardType.NewStationary);
        }
        for (int i = 0; i < 7; i++)
        {
            dailyCardTypes.Add(DailyCardType.NewSticker);
        }
        for (int i = 0; i < 3; i++)
        {
            dailyCardTypes.Add(DailyCardType.NewBadge);
        }
    }

    /// <summary>
    /// 일일상점 데일리카드 타입 섞기
    /// 학용품조각
    /// 스티커 조각
    /// 뱃지 조각
    /// 신규 학용품
    /// 신규 스티커
    /// 신규 뱃지 
    /// </summary>
    private void Shuffle2(/*int typeLength, int cardCount*/)
    {
        int enumLength = Enum.GetValues(typeof(DailyCardType)).Length;
        int index1, index2;
        DailyCardType temp;
        for (int i = 0; i < 5000; i++)
        {
            index1 = Random.Range(0, enumLength);
            index2 = Random.Range(0, enumLength);
            temp = dailyCardTypes[index1];
            dailyCardTypes[index1] = dailyCardTypes[index2];
            dailyCardTypes[index2] = temp;
        }
    }

    private void ShowDailyCard()
    {
        Shuffle2();
        // 유료 카드 생성
        for (int i = 0; i < dailyCardCount - 1; i++)
        {
            Debug.Log("상점카드 생성");
            DailyItem paidDailyCard = Instantiate(dailyItemPrefab, transform);
            Check(dailyCardTypes[i]);
            paidDailyCard.SetCardInfo(dailyCardTypes[i], 10);
        }
    }
    enum StationaryType
    {

    }
    enum StickerType
    {

    }
    enum Badge
    {
        
    }


    private void Check(DailyCardType dailyCardType)
    {
            switch (dailyCardType)
            {
                case DailyCardType.StationerySheet:
                ItemType.
                    // 학용품 13개 조각 중 랜덤으로 하나
                    break;
                case DailyCardType.StickerSheet:
                    // 스티커 32개 조각 중 랜덤으로 하나 
                    break;
                case DailyCardType.BadgeSheet:
                    // 뱃지 13개 조각 중 랜덤으로 하나 
                    break;
                case DailyCardType.NewStationary:

                    break;
                case DailyCardType.NewSticker:

                    break;
                case DailyCardType.NewBadge:

                    break;
            }
    }

    private void CheckItemSheet()
    {

    }
    private void ShowFreeDailyCard()
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
        //freeDailyCard.SetCardInfo(dailyCardTypes[0], freeItemCount);
    }


    private void Shuffle()
    {


        int index1, index2;
        DailyCardType temp;
        for (int i = 0; i < 50; i++)
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
