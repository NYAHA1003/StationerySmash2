using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
{
    //ī�� ������ ����
    public List<CardSaveData> _unitSaveDatas = new List<CardSaveData>();
    //������ �ִ� ��Ų
    public List<SkinData> _haveSkinList = new List<SkinData>();
    //���� ������
    public ProfileType _currentProfileType = ProfileType.None;
    //������ �ִ� ������
    public List<ProfileType> _haveProfileList = new List<ProfileType>();
    //������ �ִ� ���
    public List<MaterialData> _materialDatas = new List<MaterialData>();
    //������ �ִ� ��
    public int _money = 0;
    //������ �ִ� �ް�
    public int _dalgona = 0;
    //�̸�
    public string _name = "";
}

[System.Serializable]
public class CardSaveData //�� ī���� ���� ������
{
    public int _level = 0;
    public CardType _cardType = CardType.Execute;
    public StarategyType _strategicType = StarategyType.None;
    public UnitType _unitType = UnitType.None;
    public StickerType stickerType = StickerType.None;
}
