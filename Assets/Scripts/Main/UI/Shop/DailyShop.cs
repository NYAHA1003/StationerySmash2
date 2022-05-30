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
        Pencil, // 연필 
        Sharp, // 샤프
        Eraser, // 지우개
        Scissors, // 가위
        Glue, // 풀
        Ruler, // 자
        Boxcutter, // 커터칼
        Postit, // 포스트잇
        Sharplead, // 샤프심
        ballpointPen, // 볼펜
        EraserPiece, // 지우개 조각
        PostitSheet // 포스트잇 조각
    }
    public enum StickersType
    {
        Magu, //마구 스티커
        Run, // 달리기 스티커
        Paper, // 보 스티커 
        Rock, // 바위 스티커 
        Scissors, // 가위 스티커
        Heal, // 힐링 스티커
        Huge, // 거대 스티커 
        Longsee, // 원시 스티커 
        GradeUp, // 강화 스티커 
        Armor, // 갑옷 스티커 
        A, // 면역 스티커
        B, // 무거운 스티커 
        C, // 흑연 스티커 
        D, // 새 연필 스티커 
        E, // HB 스티커 
        F, // 강철 스티커 
        G, // 질긴 고무 스티커 
        H, // 조각 스티커 
        I, // 조리용 가위 스티커 
        J, // 핑킹가위  스티커 
        K, // 다 쓴 풀 스티커 
        L, // 물풀 스티커  
        M, // 경량 플라스틱 스티커 
        N, // 녹슨 쇠 스티커 
        O, // 찢는 날 스티커 
        P, // 날카로운 칼날 스티커 
        Q, // 여러 장 스티커
        R, // 마지막 장 스티커
        S, // 특제 심 스티커
        T, // 저격 스티커
        U, // 특제 플라스틱 스티커
        V, //치명적인 잉크 스티커
    }
    public enum Badge
    {
        Health, // 튼튼한 재질
        Discount, // 바겐 세일
        Increase, // 특대형
        TimeUp, // ㅈ↑퍼
        TimeDown, // ㅈ↓퍼
        Magnet_N, // 자석 N
        Magnet_S, // 자석 S
        OnleOne, // 단 "1"
        Blanket, // 이불밖은 위험해
        A, // 군것질
        B, // 다마고치
        C, // 씨앗
        Thorn, // 가시 천 
    }

}


public class DailyShop : MonoBehaviour
{
    [SerializeField]
    private DailyItem _dailyItemPrefab;

    // 나올 아이템 조각들의 데이터 
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

    private List<DailyCardType> _dailyCardTypes = new List<DailyCardType>(); // 일일상점 카드 타입 
    private List<DailyItem> _dailyItems = new List<DailyItem>(); // 6개의 일일상점 아이템 

    [SerializeField]
    private List<StationeryType> _notHaveStationaryTypes = new List<StationeryType>(); // 보유하고 있지 않은 학용품리스트
    [SerializeField]
    private List<StickersType> _notHaveStickersTypes = new List<StickersType>(); // 보유하고 있지 않은 스티커리스트
    [SerializeField]
    private List<BadgeType> _notHaveBadgeTypes = new List<BadgeType>(); // 보유하고 있지 않은 뱃지리스트 

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

    [ContextMenu("새로운 카드 데이터 넣어주기")]
    public void OnTest()
    {
        UpdateNotHaveItems();
        ShowDailyCard();
        ShowFreeDailyCard();
    }
    /// <summary>
    /// 보유하고 있지 않은 아이템들 넣어주기 
    /// </summary>
    private void UpdateNotHaveItems()
    {
        _notHaveStationaryTypes.Clear();
        _notHaveStickersTypes.Clear();
        _notHaveBadgeTypes.Clear();

        // 임시 코드 (나중에 지울거야)
        _notHaveStationaryTypes.Add(StationeryType.Pencil);
        _notHaveStationaryTypes.Add(StationeryType.Sharp);
        _notHaveStationaryTypes.Add(StationeryType.Eraser);

        _notHaveStickersTypes.Add(StickersType.Magu);
        _notHaveStickersTypes.Add(StickersType.Run);
        _notHaveStickersTypes.Add(StickersType.Paper);

        _notHaveBadgeTypes.Add(BadgeType.Health);
        _notHaveBadgeTypes.Add(BadgeType.Discount);
        _notHaveBadgeTypes.Add(BadgeType.Increase);
        // 임시 코드 끝

        // 나중에 유저 데이터에서 받아올거야 
        int stationeryTypeLength = Enum.GetValues(typeof(StationeryType)).Length;
        int stickerTypeLength = Enum.GetValues(typeof(StickersType)).Length;
        int badgeTypeLength = Enum.GetValues(typeof(BadgeType)).Length;

        for (int i = 0; i < stationeryTypeLength; i++)
        {
            //if(유저.가지고있지않은 학용품 == (StationaryType)i) 
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
    /// 일일상점 카드타입 확률에따라 넣어주기
    /// 60% : 학용품 조각
    /// 25% : 스티커 조각
    /// 13% : 뱃지 조각
    /// 1% : 신규 학용품
    /// 0.7% : 신규 스티커
    /// 0.3% : 신규 뱃지
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
    /// 일일상점 데일리카드 타입 섞기
    /// 학용품조각
    /// 스티커 조각
    /// 뱃지 조각
    /// 신규 학용품
    /// 신규 스티커
    /// 신규 뱃지 
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
    /// 아이템 카드 생성  
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
        // 유료 카드 생성 0 : 무료 카드, 1~ 5 : 유료카드 
        for (int i = 0; i < _dailyCardCount - 1; i++)
        {
            Debug.Log("상점카드 생성");
            DailyItem paidDailyCard = _dailyItems[i + 1];


            // Check에서 switch문 돌리지 말고 여기에서 switch문 돌린 후 
            // 각각 자기를 반환 하게 한후 ( 반환형을 바꾼다 StationerySheet, StickerSheet 같이 ) 
            // ReturnItemInfo와 ReturnRandomCount 로 아이템 정보와 개수 얻어오는 형태로 ]


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
    /// 유료 카드 생성 확인 
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
            // 아래 두줄은 나중에 StationerySheet를 부모 클래스(ReturnItemInfo() 보유) 상속 받게 해서
            // int length 밑에 부모 클래스 선언 후 case문 돌려서 할당 후 
            // 밑에서 ReturnItemInfo로 반환할거야 

            // 학용품 13개 조각 중 랜덤으로 하나

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
                    Debug.LogError("스티커 뽑기 에러");
                }
                // 일반 레어 에픽 4 : 2 : 1 

                StickerSheet newSticker = new StickerSheet(_stickerSheetSO, stickerSheetGrade);
                return newSticker.ReturnRandomSticker();
            //return stickerSheet.ReturnItemInfo(StickerType.Rock);
            // 스티커 32개 조각 중 랜덤으로 하나 

            case DailyCardType.BadgeSheet:
                Grade badgeSheetGrade = Grade.Common;

                commonPercent = 63; rarePercent = 21; epicPercent = 7;
                randomGrade = Random.Range(1, 101);
                // 일반 레어 에픽 63 : 21 : 7  

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
                    //Debug.LogError("스티커 뽑기 에러");
                }
                // 일반 레어 에픽 64 : 28 : 8 
                BadgeSheet badgeSheet = new BadgeSheet(_badgeSheetSO, badgeSheetGrade);
                return badgeSheet.ReturnRandomBadge();
            // 뱃지 13개 조각 중 랜덤으로 하나 

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
                    Debug.LogError("스티커 뽑기 에러");
                }
                // 일반 레어 에픽 4 : 2 : 1 

                NewSticker stickerSheet = new NewSticker(_newStickerSheetSO, newStickerGrade);
                return stickerSheet.ReturnRandomSticker();
            //return stickerSheet.ReturnItemInfo(StickerType.Rock);
            // 스티커 32개 조각 중 랜덤으로 하나 

            case DailyCardType.NewBadge:

                Grade newBadgeGrade = Grade.Common;

                commonPercent = 15; rarePercent = 10; epicPercent = 5;
                randomGrade = Random.Range(1, 31);
                // 일반 레어 에픽 63 : 21 : 7  

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
                    Debug.LogError("스티커 뽑기 에러");
                }

                // 일반 레어 에픽 15 : 10 : 5 
                NewBadge newBadge = new NewBadge(_badgeSheetSO, newBadgeGrade);
                return newBadge.ReturnRandomBadge();
                // 뱃지 13개 조각 중 랜덤으로 하나 
        }
        DailyItemInfo dailyItemInfo = new DailyItemInfo(); // 밑 두 줄은 지울거임 
        return dailyItemInfo;
    }


    /// <summary>
    ///  무료 카드 생성
    /// </summary>
    private void ShowFreeDailyCard()
    {
        // 무료 카드 생성 
        //int freeItemCount; // 무료템 개수
        int freeItemPercent; // 무료템 확률
        DailyFreeItemType dailyFreeItemType;
        freeItemPercent = Random.Range(0, 10);
        if (freeItemPercent > 3) // 70%확률로 골드 
        {
            dailyFreeItemType = DailyFreeItemType.Gold;
        }
        else // 30%확률로 달고나 
        {
            dailyFreeItemType = DailyFreeItemType.Dalgona;
        }
        FreeItem freeItem = new FreeItem(_freeItemSO, dailyFreeItemType);
        //  freeItemCount = freeItem.ReturnRandomCount();
        DailyItem freeDailyCard = _dailyItems[0];
        freeDailyCard.SetCardInfo(freeItem.ReturnItemInfo(dailyFreeItemType));
    }
}
