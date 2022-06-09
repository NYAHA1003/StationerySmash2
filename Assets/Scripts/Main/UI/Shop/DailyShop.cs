using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Utill.Data;
using System;

namespace Utill.Data
{
    public enum StationeryType
    {
        Pencil, // ���� 
        Sharp, // ����
        Eraser, // ���찳
        Scissors, // ����
        Glue, // Ǯ
        Ruler, // ��
        Boxcutter, // Ŀ��Į
        Postit, // ����Ʈ��
        Sharplead, // ������
        ballpointPen, // ����
        EraserPiece, // ���찳 ����
        PostitSheet // ����Ʈ�� ����
    }
    public enum StickersType
    {
        Magu, //���� ��ƼĿ
        Run, // �޸��� ��ƼĿ
        Paper, // �� ��ƼĿ 
        Rock, // ���� ��ƼĿ 
        Scissors, // ���� ��ƼĿ
        Heal, // ���� ��ƼĿ
        Huge, // �Ŵ� ��ƼĿ 
        Longsee, // ���� ��ƼĿ 
        GradeUp, // ��ȭ ��ƼĿ 
        Armor, // ���� ��ƼĿ 
        A, // �鿪 ��ƼĿ
        B, // ���ſ� ��ƼĿ 
        C, // �濬 ��ƼĿ 
        D, // �� ���� ��ƼĿ 
        E, // HB ��ƼĿ 
        F, // ��ö ��ƼĿ 
        G, // ���� �� ��ƼĿ 
        H, // ���� ��ƼĿ 
        I, // ������ ���� ��ƼĿ 
        J, // ��ŷ����  ��ƼĿ 
        K, // �� �� Ǯ ��ƼĿ 
        L, // ��Ǯ ��ƼĿ  
        M, // �淮 �ö�ƽ ��ƼĿ 
        N, // �콼 �� ��ƼĿ 
        O, // ���� �� ��ƼĿ 
        P, // ��ī�ο� Į�� ��ƼĿ 
        Q, // ���� �� ��ƼĿ
        R, // ������ �� ��ƼĿ
        S, // Ư�� �� ��ƼĿ
        T, // ���� ��ƼĿ
        U, // Ư�� �ö�ƽ ��ƼĿ
        V, //ġ������ ��ũ ��ƼĿ
    }
    public enum Badge
    {
        Health, // ưư�� ����
        Discount, // �ٰ� ����
        Increase, // Ư����
        TimeUp, // ������
        TimeDown, // ������
        Magnet_N, // �ڼ� N
        Magnet_S, // �ڼ� S
        OnleOne, // �� "1"
        Blanket, // �̺ҹ��� ������
        A, // ������
        B, // �ٸ���ġ
        C, // ����
        Thorn, // ���� õ 
    }

}


public class DailyShop : MonoBehaviour
{
    [SerializeField]
    private DailyItem _dailyItemPrefab;

    // ���� ������ �������� ������ 
    [SerializeField]
    private DailyItemSO _freeItemSO;
    [SerializeField]
    private DailyItemSO _stationerySheetSO;
    [SerializeField]
    private DailyItemSO _stickerSheetSO;
    [SerializeField]
    private DailyItemSO _badgeSheetSO;
    [SerializeField]
    private DailyItemSO _newStationerySO;
    [SerializeField]
    private DailyItemSO _newStickerSheetSO;
    [SerializeField]
    private DailyItemSO _newBadgeSheetSO;

    private List<DailyCardType> _dailyCardTypes = new List<DailyCardType>(); // ���ϻ��� ī�� Ÿ�� 
    private List<DailyItem> _dailyItems = new List<DailyItem>(); // 6���� ���ϻ��� ������ 

    [SerializeField]
    private List<StationeryType> _notHaveStationaryTypes = new List<StationeryType>(); // �����ϰ� ���� ���� �п�ǰ����Ʈ
    [SerializeField]
    private List<StickersType> _notHaveStickersTypes = new List<StickersType>(); // �����ϰ� ���� ���� ��ƼĿ����Ʈ
    [SerializeField]
    private List<BadgeType> _notHaveBadgeTypes = new List<BadgeType>(); // �����ϰ� ���� ���� ��������Ʈ 

    private int _dailyCardCount = 6;
    private void Start()
    {
        CreateItems();
        InitializeDailyCards();
        UpdateNotHaveItems();
        ShowFreeDailyCard();    
        ShowDailyCard();
        //Shuffle();
        //CreateDailyItem();
    }

    [ContextMenu("���ο� ī�� ������ �־��ֱ�")]
    public void OnTest()
    {
        UpdateNotHaveItems();
        ShowDailyCard();
        ShowFreeDailyCard();
    }
    /// <summary>
    /// �����ϰ� ���� ���� �����۵� �־��ֱ� 
    /// </summary>
    private void UpdateNotHaveItems()
    {
        _notHaveStationaryTypes.Clear();
        _notHaveStickersTypes.Clear();
        _notHaveBadgeTypes.Clear();

        // �ӽ� �ڵ� (���߿� ����ž�)
        _notHaveStationaryTypes.Add(StationeryType.Pencil);
        _notHaveStationaryTypes.Add(StationeryType.Sharp);
        _notHaveStationaryTypes.Add(StationeryType.Eraser);

        _notHaveStickersTypes.Add(StickersType.Magu);
        _notHaveStickersTypes.Add(StickersType.Run);
        _notHaveStickersTypes.Add(StickersType.Paper);

        _notHaveBadgeTypes.Add(BadgeType.Health);
        _notHaveBadgeTypes.Add(BadgeType.Discount);
        _notHaveBadgeTypes.Add(BadgeType.Increase);
        // �ӽ� �ڵ� ��

        // ���߿� ���� �����Ϳ��� �޾ƿðž� 
        int stationeryTypeLength = Enum.GetValues(typeof(StationeryType)).Length;
        int stickerTypeLength = Enum.GetValues(typeof(StickersType)).Length;
        int badgeTypeLength = Enum.GetValues(typeof(BadgeType)).Length;

        for (int i = 0; i < stationeryTypeLength; i++)
        {
            //if(����.�������������� �п�ǰ == (StationaryType)i) 
            // notHaveStationaryTypes.Add((StationaryType)i);
        }
        for (int i = 0; i < stickerTypeLength; i++)
        {

        }
        for (int i = 0; i < badgeTypeLength; i++)
        {

        }

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
    private void InitializeDailyCards()
    {
        for (int i = 0; i < 600; i++)
        {
            _dailyCardTypes.Add(DailyCardType.StationerySheet);
        }
        for (int i = 0; i < 250; i++)
        {
            _dailyCardTypes.Add(DailyCardType.StickerSheet);
        }
        for (int i = 0; i < 130; i++)
        {
            _dailyCardTypes.Add(DailyCardType.BadgeSheet);
        }
        for (int i = 0; i < 10; i++)
        {
            _dailyCardTypes.Add(DailyCardType.NewStationary);
        }
        for (int i = 0; i < 7; i++)
        {
            _dailyCardTypes.Add(DailyCardType.NewSticker);
        }
        for (int i = 0; i < 3; i++)
        {
            _dailyCardTypes.Add(DailyCardType.NewBadge);
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
    private void Shuffle(/*int typeLength, int cardCount*/)
    {
        int length = _dailyCardTypes.Count;
        int index1, index2;
        DailyCardType temp;
        for (int i = 0; i < 5000; i++)
        {
            index1 = Random.Range(0, length);
            index2 = Random.Range(0, length);
            temp = _dailyCardTypes[index1];
            _dailyCardTypes[index1] = _dailyCardTypes[index2];
            _dailyCardTypes[index2] = temp;
        }
    }

    /// <summary>
    /// ������ ī�� ����  
    /// </summary>
    private void CreateItems()
    {
        for (int i = 0; i < _dailyCardCount; i++)
        {
            DailyItem dailyItem = Instantiate(_dailyItemPrefab, transform);
            _dailyItems.Add(dailyItem);
        }
    }
    private void ShowDailyCard()
    {
        Shuffle();
        // ���� ī�� ���� 0 : ���� ī��, 1~ 5 : ����ī�� 
        for (int i = 0; i < _dailyCardCount - 1; i++)
        {
            Debug.Log("����ī�� ����");
            DailyItem paidDailyCard = _dailyItems[i + 1];


            // Check���� switch�� ������ ���� ���⿡�� switch�� ���� �� 
            // ���� �ڱ⸦ ��ȯ �ϰ� ���� ( ��ȯ���� �ٲ۴� StationerySheet, StickerSheet ���� ) 
            // ReturnItemInfo�� ReturnRandomCount �� ������ ������ ���� ������ ���·� ]


            //switch (_dailyCardTypes[i])
            //{
            //    case DailyCardType.StationerySheet:
            //        StationerySheet stationeryDailyCard = Check(_dailyCardTypes[i]);
            //        paidDailyCard.SetCardInfo(stationeryDailyCard.ReturnItemInfo());
            //        break;
            //    case DailyCardType.StickerSheet:
            //        StickerSheet stickerSheetDailyCard = Check(_dailyCardTypes[i]);
            //        break;
            //    case DailyCardType.BadgeSheet:
            //        BadgeSheet badgeSheetDailyCard = Check(_dailyCardTypes[i]);
            //        break;
            //    case DailyCardType.NewStationary:
            //        NewStationery newStationeryDailyCard = Check(_dailyCardTypes[i]);
            //        break;
            //    case DailyCardType.NewSticker:
            //        NewSticker newStickerDailyCard = Check(_dailyCardTypes[i]);
            //        break;
            //    case DailyCardType.NewBadge:
            //        NewBadge newBadgeDailyCard = Check(_dailyCardTypes[i]);
            //        break;
            //}

            DailyItemInfo dailyItemInfo = Check(_dailyCardTypes[i]);
            paidDailyCard.SetCardInfo(dailyItemInfo);
        }
    }

    /// <summary>
    /// ���� ī�� ���� Ȯ�� 
    /// </summary>
    /// <param name="dailyCardType"></param>
    /// <returns></returns>
    private DailyItemInfo Check(DailyCardType dailyCardType)
    {
        int length;
        int commonPercent, rarePercent, epicPercent, randomGrade;
        Debug.Log(dailyCardType);
        switch (dailyCardType)
        {
            case DailyCardType.StationerySheet:
                length = Enum.GetValues(typeof(StationeryType)).Length;
                StationeryType stationaryType = (StationeryType)(Random.Range(0, length));
                StationerySheet stationerySheet = new StationerySheet(_stationerySheetSO);
                Debug.Log(stationerySheet.ReturnItemInfo(stationaryType)._cardPrice);
                return stationerySheet.ReturnItemInfo(stationaryType);
            // �Ʒ� ������ ���߿� StationerySheet�� �θ� Ŭ����(ReturnItemInfo() ����) ��� �ް� �ؼ�
            // int length �ؿ� �θ� Ŭ���� ���� �� case�� ������ �Ҵ� �� 
            // �ؿ��� ReturnItemInfo�� ��ȯ�Ұž� 

            // �п�ǰ 13�� ���� �� �������� �ϳ�

            case DailyCardType.StickerSheet:

                Grade stickerSheetGrade = Grade.Common;

                commonPercent = 4; rarePercent = 2; epicPercent = 1;
                randomGrade = Random.Range(1, 8);

                if (randomGrade <= commonPercent)
                {
                    stickerSheetGrade = Grade.Common;
                }
                else if (randomGrade <= commonPercent + rarePercent)
                {
                    stickerSheetGrade = Grade.Rare;
                }
                else if (randomGrade <= commonPercent + rarePercent + epicPercent)
                {
                    stickerSheetGrade = Grade.Epic;
                }
                else
                {
                    Debug.LogError("��ƼĿ �̱� ����");
                }
                // �Ϲ� ���� ���� 4 : 2 : 1 

                StickerSheet newSticker = new StickerSheet(_stickerSheetSO, stickerSheetGrade);
                return newSticker.ReturnRandomSticker();
            //return stickerSheet.ReturnItemInfo(StickerType.Rock);
            // ��ƼĿ 32�� ���� �� �������� �ϳ� 

            case DailyCardType.BadgeSheet:
                Grade badgeSheetGrade = Grade.Common;

                commonPercent = 63; rarePercent = 21; epicPercent = 7;
                randomGrade = Random.Range(1, 101);
                // �Ϲ� ���� ���� 63 : 21 : 7  

                if (randomGrade <= commonPercent)
                {
                    badgeSheetGrade = Grade.Common;
                }
                else if (randomGrade <= commonPercent + rarePercent)
                {
                    badgeSheetGrade = Grade.Rare;
                }
                else if (randomGrade <= commonPercent + rarePercent + epicPercent)
                {
                    badgeSheetGrade = Grade.Epic;
                }
                else
                {
                    //Debug.LogError("��ƼĿ �̱� ����");
                }
                // �Ϲ� ���� ���� 64 : 28 : 8 
                BadgeSheet badgeSheet = new BadgeSheet(_badgeSheetSO, badgeSheetGrade);
                return badgeSheet.ReturnRandomBadge();
            // ���� 13�� ���� �� �������� �ϳ� 

            case DailyCardType.NewStationary:

                length = _notHaveStationaryTypes.Count;
                int index = Random.Range(0, length);
                StationeryType newStationaryType = _notHaveStationaryTypes[index];
                NewStationery newStationery = new NewStationery(_newStationerySO);
                return newStationery.ReturnItemInfo(newStationaryType);

            case DailyCardType.NewSticker:

                Grade newStickerGrade = Grade.Common;

                commonPercent = 4; rarePercent = 2; epicPercent = 1;
                randomGrade = Random.Range(1, 8);

                if (randomGrade <= commonPercent)
                {
                    newStickerGrade = Grade.Common;
                }
                else if (randomGrade <= commonPercent + rarePercent)
                {
                    newStickerGrade = Grade.Rare;
                }
                else if (randomGrade <= commonPercent + rarePercent + epicPercent)
                {   
                    newStickerGrade = Grade.Epic;
                }
                else
                {
                    Debug.LogError("��ƼĿ �̱� ����");
                }
                // �Ϲ� ���� ���� 4 : 2 : 1 

                NewSticker stickerSheet = new NewSticker(_newStickerSheetSO, newStickerGrade);
                return stickerSheet.ReturnRandomSticker();
            //return stickerSheet.ReturnItemInfo(StickerType.Rock);
            // ��ƼĿ 32�� ���� �� �������� �ϳ� 

            case DailyCardType.NewBadge:

                Grade newBadgeGrade = Grade.Common;

                commonPercent = 15; rarePercent = 10; epicPercent = 5;
                randomGrade = Random.Range(1, 31);
                // �Ϲ� ���� ���� 63 : 21 : 7  

                if (randomGrade <= commonPercent)
                {
                    newBadgeGrade = Grade.Common;
                }
                else if (randomGrade <= commonPercent + rarePercent)
                {
                    newBadgeGrade = Grade.Rare;
                }
                else if (randomGrade <= commonPercent + rarePercent + epicPercent)
                {
                    newBadgeGrade = Grade.Epic;
                }
                else
                {
                    Debug.LogError("��ƼĿ �̱� ����");
                }

                // �Ϲ� ���� ���� 15 : 10 : 5 
                NewBadge newBadge = new NewBadge(_badgeSheetSO, newBadgeGrade);
                return newBadge.ReturnRandomBadge();
                // ���� 13�� ���� �� �������� �ϳ� 
        }
        DailyItemInfo dailyItemInfo = new DailyItemInfo(); // �� �� ���� ������� 
        return dailyItemInfo;
    }


    /// <summary>
    ///  ���� ī�� ����
    /// </summary>
    private void ShowFreeDailyCard()
    {
        // ���� ī�� ���� 
        //int freeItemCount; // ������ ����
        int freeItemPercent; // ������ Ȯ��
        DailyFreeItemType dailyFreeItemType;
        freeItemPercent = Random.Range(0, 10);
        if (freeItemPercent > 3) // 70%Ȯ���� ��� 
        {
            dailyFreeItemType = DailyFreeItemType.Gold;
        }
        else // 30%Ȯ���� �ް� 
        {
            dailyFreeItemType = DailyFreeItemType.Dalgona;
        }
        FreeItem freeItem = new FreeItem(_freeItemSO, dailyFreeItemType);
        //  freeItemCount = freeItem.ReturnRandomCount();
        DailyItem freeDailyCard = _dailyItems[0];
        freeDailyCard.SetCardInfo(freeItem.ReturnItemInfo(dailyFreeItemType));
    }
}
