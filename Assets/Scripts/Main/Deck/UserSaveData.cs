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
        public List<CardSaveData> _unitSaveDatas = new List<CardSaveData>();    //ī�� ������ ����
        public List<CardSaveData> _ingameSaveDatas = new List<CardSaveData>();    //�ΰ��ӵ� ī�� ������ ����
        public List<SkinType> _haveSkinList = new List<SkinType>();    //������ �ִ� ��Ų
        public List<PencilCaseType> _havePencilCaseList = new List<PencilCaseType>();    //������ �ִ� ����
        public PencilCaseType _currentPencilCaseType = PencilCaseType.Normal; // ���� ������ ����
        public List<BadgeSaveData> _badgeSaveDatas = new List<BadgeSaveData>();
        public ProfileType _currentProfileType = ProfileType.ProNone;    //���� ������
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //������ �ִ� ������ ���
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //������ �ִ� ��� ���
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //������ �ִ� ���
        public int _money = 0;    //������ �ִ� ��
        public int _dalgona = 0;    //������ �ִ� �ް�
        public string _name = "";    //�̸�
        public int _level = 1; //����
        public int _nowExp = 0; //���� ����ġ
        public StageType _lastPlayStage = StageType.None; //���������� �÷����� ��������
        public int _winCount = 0; //�¸��� Ƚ��
        public int _winningStreakCount = 0; //���� ũ�� ������ Ƚ��
        public int _loseCount = 0; //�й��� Ƚ��
    }

    [System.Serializable]
    public class CardSaveData //�� ī���� ���� ������
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