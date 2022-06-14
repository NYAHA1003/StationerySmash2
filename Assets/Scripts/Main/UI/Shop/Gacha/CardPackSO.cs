using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Gacha/CardPackSO")]
public class CardPackSO : ScriptableObject
{
    [Header("0 : 허름한 카드팩, 1 : 빛나는 카드팩, 2 : 레전더리 카드팩")]
    public List<CardPackInfo> cardPackInfos = new List<CardPackInfo>(); 
}

public class CardPackInfo
{
    public int amount; // 나올 분실물 조각 총량 
    public int minCount = 0;               //팩에서 나오는 분실물 조각 종류 최소 갯수
    public int maxCount= 0;               //팩에서 나오는 분실물 조각 종류 최대 갯수
    public int newCardPercent = 0;        //신규 캐릭터가 나올 확률(팩당)
    public int useDalgona = 0;        //사용하는 달고나 갯수
}

