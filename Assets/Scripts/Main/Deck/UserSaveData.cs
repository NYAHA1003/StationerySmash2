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
        public List<SkinType> _haveSkinList = new List<SkinType>();    //������ �ִ� ��Ų
        public List<StickerSaveData> _haveStickerList = new List<StickerSaveData>(); //������ �ִ� ��ƼĿ
        public List<PencilCaseSaveData> _havePencilCaseList = new List<PencilCaseSaveData>();    //������ �ִ� ����
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
        public ThemeSkinType _themeSkinType = ThemeSkinType.Normal; // ���������� ������ �׸� Ÿ��
        public List<ThemeSkinType> _haveThemeSkinTypeList = new List<ThemeSkinType>(); // ���� �׸� Ÿ��

        public List<CardNamingType> _presetCardDatas1 = new List<CardNamingType>();
        public List<CardNamingType> _presetCardDatas2 = new List<CardNamingType>();
        public List<CardNamingType> _presetCardDatas3 = new List<CardNamingType>();
        public PencilCaseType _presetPencilCaseType1 = PencilCaseType.Normal;
        public PencilCaseType _presetPencilCaseType2 = PencilCaseType.Normal;
        public PencilCaseType _presetPencilCaseType3 = PencilCaseType.Normal;

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
            _money += money;
		}

        /// <summary>
        /// �ΰ��� �����͸� ������ �����Ϳ� ���� �����Ѵ�
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
        /// ���� ī�� ���� ��ȯ�Ѵ�
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
        /// �����¿� ī�带 �߰��Ѵ�
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
        /// �����¿� ī�带 �����Ѵ�
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
    public class CardSaveData //�� ī���� ���� ������
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