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
    private List<ItemType> itemTypes = new List<ItemType>(); // ���ϻ��� ������Ÿ��
    private List<DailyCardType> dailyCardTypes = new List<DailyCardType>(); // ���ϻ��� ī�� Ÿ��

    // �ӽ� ī�� ���� ����
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
    // ��

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
    /// ���ϻ��� ī��Ÿ�� Ȯ�������� �־��ֱ�
    /// 60% : �п�ǰ ����
    /// 25% : ��ƼĿ ����
    /// 13% : ���� ����
    /// 1% : �ű� �п�ǰ
    /// 0.7% : �ű� ��ƼĿ
    /// 0.3% : �ű� ����
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
    /// ���ϻ��� ���ϸ�ī�� Ÿ�� ����
    /// �п�ǰ����
    /// ��ƼĿ ����
    /// ���� ����
    /// �ű� �п�ǰ
    /// �ű� ��ƼĿ
    /// �ű� ���� 
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
        // ���� ī�� ����
        for (int i = 0; i < dailyCardCount - 1; i++)
        {
            Debug.Log("����ī�� ����");
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
                    // �п�ǰ 13�� ���� �� �������� �ϳ�
                    break;
                case DailyCardType.StickerSheet:
                    // ��ƼĿ 32�� ���� �� �������� �ϳ� 
                    break;
                case DailyCardType.BadgeSheet:
                    // ���� 13�� ���� �� �������� �ϳ� 
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
        // ���� ī�� ���� 
        int freeItemCount; // ������ ����
        int freeItemPercent; // ������ Ȯ��
        DailyFreeItemType dailyFreeItemType;
        freeItemPercent = Random.Range(0, 10);
        if (freeItemPercent > 3) // 70%Ȯ���� ��� 
        {
            dailyFreeItemType = DailyFreeItemType.Gold;
            freeItemCount = Random.Range(10, 21) * 10;
        }
        else // 30%Ȯ���� �ް� 
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
    /// ����ī�� ���� 
    /// </summary>
    private void CreateDailyItem()
    {
        // ���� ī�� ���� 
        int freeItemCount; // ������ ����
        int freeItemPercent; // ������ Ȯ��
        DailyFreeItemType dailyFreeItemType;
        freeItemPercent = Random.Range(0, 10);
        if (freeItemPercent > 3) // 70%Ȯ���� ��� 
        {
            dailyFreeItemType = DailyFreeItemType.Gold;
            freeItemCount = Random.Range(10, 21) * 10;
        }
        else // 30%Ȯ���� �ް� 
        {
            dailyFreeItemType = DailyFreeItemType.Dalgona;
            freeItemCount = Random.Range(3, 7);
        }
        DailyItem freeDailyCard = Instantiate(dailyItemPrefab, transform);
        freeDailyCard.SetCardInfo(dailyCardTypes[0], freeItemCount);


        int paidItemCount; // ������ ����
        int paidItemPercent; // ������ Ȯ��
        DailyCardType dailyCardType;

        // ���� ī�� ����
        for (int i = 0; i < dailyCardCount; i++)
        {
            DailyItem paidDailyCard = Instantiate(dailyItemPrefab, transform);
            //paidDailyCard.SetCardInfo(dailyCardTypes[i]);
        }
    }

}
