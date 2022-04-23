using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
{
    //ī�� ������ ����
    public List<SaveData> _unitSaveDatas = new List<SaveData>();
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
public class SaveData //�� ī���� ���� ������
{
    public int _level;
    public CardType _cardType;
    public StarategyType _strategicType;
    public UnitType _unitType;
}
