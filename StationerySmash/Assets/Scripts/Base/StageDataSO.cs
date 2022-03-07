using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Object/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [Header("임시 유닛 데이터")]
    public List<StageData> stageDatas;
}

[System.Serializable]
public class StageData
{
    public string name;
    public float max_Range;
}