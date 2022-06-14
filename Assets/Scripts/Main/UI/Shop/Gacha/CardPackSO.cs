using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Gacha/CardPackSO")]
public class CardPackSO : ScriptableObject
{
    [Header("0 : �㸧�� ī����, 1 : ������ ī����, 2 : �������� ī����")]
    public List<CardPackInfo> cardPackInfos = new List<CardPackInfo>(); 
}

public class CardPackInfo
{
    public int amount; // ���� �нǹ� ���� �ѷ� 
    public int minCount = 0;               //�ѿ��� ������ �нǹ� ���� ���� �ּ� ����
    public int maxCount= 0;               //�ѿ��� ������ �нǹ� ���� ���� �ִ� ����
    public int newCardPercent = 0;        //�ű� ĳ���Ͱ� ���� Ȯ��(�Ѵ�)
    public int useDalgona = 0;        //����ϴ� �ް� ����
}

