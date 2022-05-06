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
        public ProfileType _currentProfileType = ProfileType.None;    //현재 프로필
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //가지고 있는 프로필
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //가지고 있는 재료
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //가지고 있는 재료
        public int _money = 0;    //가지고 있는 돈
        public int _dalgona = 0;    //가지고 있는 달고나
        public string _name = "";    //이름
        public int _level = 1;
        public int _nowExp = 0;
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
}