using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[CreateAssetMenu(menuName = "SO/Gacha/CardPackSO")]
public class CardPackSO : ScriptableObject
{
    [Header("0 : �㸧�� ī���� , 1 : ������ ī����, 2 : �������� ī����")]
    public List<CardPackInfo> cardPackInfos = new List<CardPackInfo>(); 
}

[Serializable]
public class CardPackInfo
{
    [Range(20,300)] public int amount; // ���� �нǹ� ���� �ѷ� 
    [Range(2,6)] public int minCount = 0;               //�ѿ��� ������ �нǹ� ���� ���� �ּ� ����
    [Range(3,8)]public int maxCount= 0;               //�ѿ��� ������ �нǹ� ���� ���� �ִ� ����
    [Range(1, 100)] public int newCardPercent = 0;        //�ű� ĳ���Ͱ� ���� Ȯ��(�Ѵ�)
    [Range(5, 45)] public int useDalgona = 0;        //����ϴ� �ް� ����
    [Range(1000, 100000)] public int useCoin = 0;        //����ϴ� �ް� ����
}

