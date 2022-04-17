using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
{
    //������ �����͵��� ����
    public List<SaveData> unitSaveDatas;
    //�κ��丮
    //��
    //ĳ�� ���
}

[System.Serializable]
public class SaveData //�� ī���� ���� ������
{
    public int _level;
    public CardType _cardType;
    public StarategyType _strategicType;
    public UnitType _unitType;
}
