using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //�κ��丮, ��, ĳ�� �̷��� ������ ���� �ִ�
{
    //������ �����͵��� ����
    public List<SaveData> unitSaveDatas;
}

[System.Serializable]
public class SaveData //�� ������ ���� ������
{
    public int _level;
    public UnitType _unitType;
}
