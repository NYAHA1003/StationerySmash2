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
        public string _userID = "";
        public List<CardSaveData> _haveCardSaveDatas = new List<CardSaveData>();    //카드 데이터 저장
        public List<SkinType> _haveSkinList = new List<SkinType>();    //가지고 있는 스킨
        public List<StickerSaveData> _haveStickerList = new List<StickerSaveData>(); //가지고 있는 스티커
        public List<PencilCaseSaveData> _havePencilCaseList = new List<PencilCaseSaveData>();    //가지고 있는 필통
        public PencilCaseType _currentPencilCaseType = PencilCaseType.Normal; // 현재 착용한 필통
        public List<BadgeSaveData> _haveBadgeSaveDatas = new List<BadgeSaveData>();
        public ProfileType _currentProfileType = ProfileType.ProNone;    //현재 프로필
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //가지고 있는 프로필 목록
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //가지고 있는 재료 목록
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //가지고 있는 재료
        public int _setPrestIndex = 0; //마지막으로 사용했던 프리셋
        public int _money = 0;    //가지고 있는 돈
        public int _dalgona = 0;    //가지고 있는 달고나
        public string _name = "";    //이름
        public int _level = 1; //레벨
        public int _nowExp = 0; //현재 경험치
        public StageType _lastPlayStage = StageType.None; //마지막으로 플레이한 스테이지
        public int _winCount = 0; //승리한 횟수
        public int _winningStreakCount = 0; //가장 크게 연승한 횟수
        public int _loseCount = 0; //패배한 횟수
        public ThemeSkinType _themeSkinType = ThemeSkinType.Normal; // 마지막으로 선택한 테마 타입
        public List<ThemeSkinType> _haveThemeSkinTypeList = new List<ThemeSkinType>(); // 가진 테마 타입

        public List<CardNamingType> _presetCardDatas1 = new List<CardNamingType>();
        public List<CardNamingType> _presetCardDatas2 = new List<CardNamingType>();
        public List<CardNamingType> _presetCardDatas3 = new List<CardNamingType>();
        public PencilCaseType _presetPencilCaseType1 = PencilCaseType.Normal;
        public PencilCaseType _presetPencilCaseType2 = PencilCaseType.Normal;
        public PencilCaseType _presetPencilCaseType3 = PencilCaseType.Normal;

        /// <summary>
        /// 경험치 증가
        /// </summary>
        public void AddExp(int exp)
		{
            _nowExp += exp;
            for(;_nowExp > _level * 100; )
            {
                if (_nowExp > (_level * 100))
                {
                    _level++;
                    _nowExp -= _level * 100;
                }
            }
            if(_nowExp < 0)
			{
                _nowExp = 0;
			}
		}

        /// <summary>
        /// 돈 증가
        /// </summary>
        public void AddMoney(int money)
		{
            if(money >= 100000000)
            {
                money = 100000000;
                return;
			}
            _money += money;
		}

        /// <summary>
        /// 인게임 데이터를 프리셋 데이터에 따라 변경한다
        /// </summary>
        public void ChangeIngameData(int preset)
		{
            switch(preset)
			{
                case 0:
                    _currentPencilCaseType = _presetPencilCaseType1;
                    break;
                case 1:
                    _currentPencilCaseType = _presetPencilCaseType2;
                    break;
                case 2:
                    _currentPencilCaseType = _presetPencilCaseType3;
                    break;
            }
            _setPrestIndex = preset;
		}

        /// <summary>
        /// 현재 카드 덱을 반환한다
        /// </summary>
        /// <returns></returns>
        public List<CardSaveData> GetIngameCardData()
		{
            List<CardNamingType> currentCardList = null;
            List<CardSaveData> cardSaveDatas = new List<CardSaveData>();
            switch (_setPrestIndex)
			{
                case 0:
                    currentCardList = _presetCardDatas1;
                    break;
                case 1:
                    currentCardList = _presetCardDatas2;
                    break;
                case 2:
                    currentCardList = _presetCardDatas3;
                    break;
            }

            int count = currentCardList.Count;
            for(int i = 0; i < count; ++i)
			{
                CardSaveData ingameData = _haveCardSaveDatas.Find(x => x._cardNamingType == currentCardList[i]);
                cardSaveDatas.Add(ingameData);
			}

            return cardSaveDatas;
        }

        /// <summary>
        /// 프리셋에 카드를 추가한다
        /// </summary>
        /// <param name="addCard"></param>
        public void AddIngameCardData(CardNamingType addCard)
        {
            List<CardNamingType> currentCardList = null;
            switch (_setPrestIndex)
            {
                case 0:
                    currentCardList = _presetCardDatas1;
                    break;
                case 1:
                    currentCardList = _presetCardDatas2;
                    break;
                case 2:
                    currentCardList = _presetCardDatas3;
                    break;
            }
            if (!currentCardList.Contains(addCard))
			{
                currentCardList.Add(addCard);
			}
        }

        /// <summary>
        /// 프리셋에 카드를 제거한다
        /// </summary>
        /// <param name="removeCard"></param>
        public void RemoveIngameCardData(CardNamingType removeCard)
        {
            List<CardNamingType> currentCardList = null;
            switch (_setPrestIndex)
            {
                case 0:
                    currentCardList = _presetCardDatas1;
                    break;
                case 1:
                    currentCardList = _presetCardDatas2;
                    break;
                case 2:
                    currentCardList = _presetCardDatas3;
                    break;
            }
            if (currentCardList.Contains(removeCard))
            {
                currentCardList.Remove(removeCard);
            }
        }
    }

    [System.Serializable]
    public class CardSaveData //한 카드의 저장 데이터
    {
        public int _level = 0;
        public int _count = 0;
        public CardNamingType _cardNamingType = CardNamingType.None;
        public SkinType _skinType = SkinType.SpriteNone;
        public StickerType _stickerType = StickerType.None;

        public static CardSaveData CopyDataToCardData(CardData cardData)
		{
            CardSaveData cardSaveData = new CardSaveData();
            cardSaveData._level = cardData._level;
            cardSaveData._count = 0;
            cardSaveData._cardNamingType = cardData._cardNamingType;
            cardSaveData._skinType = cardData._skinData._skinType;

            UnitData unitData = UnitDataManagerSO.FindUnitData(cardData._unitType);
            cardSaveData._stickerType = unitData._stickerType;
            return cardSaveData;
		}
    }
}