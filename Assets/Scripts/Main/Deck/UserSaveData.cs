using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Main.Deck
{
    [System.Serializable]
    public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
    {
        public string _userID = "";
        public List<CardSaveData> _haveCardSaveDatas = new List<CardSaveData>();    //ī�� ������ ����
        public List<CardSaveData> _ingameSaveDatas = new List<CardSaveData>();    //�ΰ��ӵ� ī�� ������ ����
        public List<SkinType> _haveSkinList = new List<SkinType>();    //������ �ִ� ��Ų
        public List<StickerSaveData> _haveStickerList = new List<StickerSaveData>(); //������ �ִ� ��ƼĿ
        public List<PencilCaseType> _havePencilCaseList = new List<PencilCaseType>();    //������ �ִ� ����
        public PencilCaseType _currentPencilCaseType = PencilCaseType.Normal; // ���� ������ ����
        public List<BadgeSaveData> _haveBadgeSaveDatas = new List<BadgeSaveData>();
        public ProfileType _currentProfileType = ProfileType.ProNone;    //���� ������
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //������ �ִ� ������ ���
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //������ �ִ� ��� ���
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //������ �ִ� ���
        public int _setPrestIndex = 0; //���������� ����ߴ� ������
        public int _money = 0;    //������ �ִ� ��
        public int _dalgona = 0;    //������ �ִ� �ް�
        public string _name = "";    //�̸�
        public int _level = 1; //����
        public int _nowExp = 0; //���� ����ġ
        public StageType _lastPlayStage = StageType.None; //���������� �÷����� ��������
        public int _winCount = 0; //�¸��� Ƚ��
        public int _winningStreakCount = 0; //���� ũ�� ������ Ƚ��
        public int _loseCount = 0; //�й��� Ƚ��
    
        /// <summary>
        /// ����ġ ����
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
        /// �� ����
        /// </summary>
        public void AddMoney(int money)
		{
            if(money >= 100000000)
            {
                money = 100000000;
                return;
			}
            _money = money;
		}
    }

    [System.Serializable]
    public class CardSaveData //�� ī���� ���� ������
    {
        public int _level = 0;
        public int _count = 0;
        public CardType _cardType = CardType.Execute;
        public CardNamingType _cardNamingType = CardNamingType.None;
        public SkinType _skinType = SkinType.SpriteNone;
        public StrategyType _strategicType = StrategyType.None;
        public UnitType _unitType = UnitType.None;
        public StickerType stickerType = StickerType.None;

        public static CardSaveData CopyDataToCardData(CardData cardData)
		{
            CardSaveData cardSaveData = new CardSaveData();
            cardSaveData._level = cardData.level;
            cardSaveData._count = 0;
            cardSaveData._cardType = cardData.cardType;
            cardSaveData._cardNamingType = cardData._cardNamingType;
            cardSaveData._skinType = cardData._skinData._skinType;
            cardSaveData._unitType = cardData.unitType;
            cardSaveData._strategicType = cardData.starategyType;

            UnitData unitData = UnitDataManagerSO.FindUnitData(cardData.unitType);
            cardSaveData.stickerType = unitData._stickerType;
            return cardSaveData;
		}
    }
}