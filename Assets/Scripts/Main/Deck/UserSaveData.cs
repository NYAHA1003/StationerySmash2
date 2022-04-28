using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Main.Deck
{
    [System.Serializable]
    public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
    {

        public List<CardSaveData> _unitSaveDatas = new List<CardSaveData>();    //ī�� ������ ����
        public List<SkinType> _haveSkinList = new List<SkinType>();    //������ �ִ� ��Ų
        public ProfileType _currentProfileType = ProfileType.None;    //���� ������
        public List<ProfileType> _haveProfileList = new List<ProfileType>();    //������ �ִ� ������
        public List<MaterialData> _materialDatas = new List<MaterialData>();    //������ �ִ� ���
        public List<CollectionThemeType> _haveCollectionDatas = new List<CollectionThemeType>();    //������ �ִ� ���
        public int _money = 0;    //������ �ִ� ��
        public int _dalgona = 0;    //������ �ִ� �ް�
        public string _name = "";    //�̸�
    }

    [System.Serializable]
    public class CardSaveData //�� ī���� ���� ������
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