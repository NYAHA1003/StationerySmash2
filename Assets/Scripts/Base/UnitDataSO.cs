using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill;

[CreateAssetMenu(fileName = "UnitDataSO", menuName = "Scriptable Object/UnitDataSO")]
public class UnitDataSO : ScriptableObject
{
    [Header("임시 유닛 데이터")]
    public List<UnitData> unitDatas;
}

[System.Serializable]
public class UnitData : DataBase
{
    public int cost;
    public int knockback;
    public float dir;
    public float moveSpeed;
    public float attackSpeed;
    public float range;
    public UnitType unitType;
}