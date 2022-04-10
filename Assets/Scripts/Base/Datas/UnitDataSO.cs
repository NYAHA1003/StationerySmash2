using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Utill;

[CreateAssetMenu(fileName = "UnitDataSO", menuName = "Scriptable Object/UnitDataSO")]
public class UnitDataSO : ScriptableObject
{
    [Header("임시 유닛 데이터")]
    public List<CardData> unitDatas;
}
