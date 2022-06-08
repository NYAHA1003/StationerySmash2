using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Gacha/GachaSO")]
public class GachaSO : ScriptableObject
{
    public float rarePercent;
    public float epicPercent;
    public int maxAmount;
}

