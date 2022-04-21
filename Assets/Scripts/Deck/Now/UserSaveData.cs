using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
{
    //������ �����͵��� ����
    public List<SaveData> unitSaveDatas;
    public List<SkinData> _haveSkinList = new List<SkinData>();
    public List<MaterialData> _materialDatas = new List<MaterialData>();
    public int _money;
    public int _dalgona;
}

[System.Serializable]
public class SaveData //�� ī���� ���� ������
{
    public int _level;
    public CardType _cardType;
    public StarategyType _strategicType;
    public UnitType _unitType;
}
