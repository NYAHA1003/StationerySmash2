using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Main.Deck
{
    [System.Serializable]
    public class UserSaveData //인벤토리, 돈, 캐시 이런걸 저장할 수도 있다
    {
        public List<CardSaveData> _unitSaveDatas = new List<CardSaveData>();    //카드 데이터 저장
        public List<CardSaveData> _ingameSaveDatas = new List<CardSaveData>();    //인게임덱 카드 데이터 저장
        public List<SkinType> _haveSkinList = new List<SkinType>();    //가지고 있는 스킨
        public List<PencilCaseType> _havePencilCaseList = new List<PencilCaseType>();    //가지고 있는 필통
        public PencilCaseType _currentPencilCaseType = PencilCaseType.Normal; // 현재 착용한 필통
        public List<BadgeSaveData> _badgeSaveDatas = new List<BadgeSaveData>();
        public ProfileType _currentProfileType = ProfileType.ProNone;    //현재 프로필
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //가지고 있는 프로필 목록
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //가지고 있는 재료 목록
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //가지고 있는 재료
        public int _money = 0;    //가지고 있는 돈
        public int _dalgona = 0;    //가지고 있는 달고나
        public string _name = "";    //이름
        public int _level = 1; //레벨
        public int _nowExp = 0; //현재 경험치
        public StageType _lastPlayStage = StageType.None; //마지막으로 플레이한 스테이지
        public int _winCount = 0; //승리한 횟수
        public int _winningStreakCount = 0; //가장 크게 연승한 횟수
        public int _loseCount = 0; //패배한 횟수
    }

    [System.Serializable]
    public class CardSaveData //한 카드의 저장 데이터
    {
        public int _level = 0;
        public int _count = 0;
        public CardType _cardType = CardType.Execute;
        public SkinType _skinType = SkinType.SpriteNone;
        public CardNamingType _cardNamingType = CardNamingType.None;
        public StarategyType _strategicType = StarategyType.None;
        public UnitType _unitType = UnitType.None;
        public StickerType stickerType = StickerType.None;

        public static CardSaveData CopyDataToCardData(CardData cardData)
		{
            CardSaveData cardSaveData = new CardSaveData();
            cardSaveData._level = cardData.level;
            cardSaveData._count = 0;
            cardSaveData._cardType = cardData.cardType;
            cardSaveData._skinType = cardData.skinData._skinType;
            cardSaveData._cardNamingType = cardData.skinData._cardNamingType;
            cardSaveData._unitType = cardData.unitData.unitType;
            cardSaveData._strategicType = cardData.strategyData.starategyType;
            cardSaveData.stickerType = cardData.unitData.stickerData._stickerType;
            return cardSaveData;
		}
    }

    [System.Serializable]
    public class BadgeSaveData
	{
        public int _level;
        public BadgeType _BadgeType;
	}
}