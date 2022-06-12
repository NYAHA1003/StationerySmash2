using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Gacha/GachaSO")]
public class GachaSO : ScriptableObject
{
    public float rarePercent; // 레어 확률
    public float epicPercent; // 영웅 확률
    public int maxAmount; // 구매 한 번에 뽑는 횟수(나올 아이템 개수) 
}

