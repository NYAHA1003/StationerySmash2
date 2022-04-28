using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Main.Deck
{
    [System.Serializable]
    public class UserSaveData //인벤토리, 돈, 캐시 이런걸 저장할 수도 있다
    {

        public List<CardSaveData> _unitSaveDatas = new List<CardSaveData>();    //카드 데이터 저장
        public List<SkinType> _haveSkinList = new List<SkinType>();    //가지고 있는 스킨
        public ProfileType _currentProfileType = ProfileType.None;    //현재 프로필
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //가지고 있는 프로필
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //가지고 있는 재료
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //가지고 있는 재료
        public int _money = 0;    //가지고 있는 돈
        public int _dalgona = 0;    //가지고 있는 달고나
        public string _name = "";    //이름
    }

    [System.Serializable]
    public class CardSaveData //한 카드의 저장 데이터
    {
        public int _level = 0;
        public int _count = 0;
        public CardType _cardType = CardType.Execute;
        public CardNamingType _cardNamingType = CardNamingType.None;
        public StarategyType _strategicType = StarategyType.None;
        public UnitType _unitType = UnitType.None;
        public StickerType stickerType = StickerType.None;
    }
}