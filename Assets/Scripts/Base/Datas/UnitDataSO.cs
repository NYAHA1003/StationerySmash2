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
    [Header("�ӽ� ���� ������")]
    public List<CardData> unitDatas;
}
