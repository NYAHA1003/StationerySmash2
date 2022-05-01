using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Object/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [Header("임시 스테이지 데이터")]
    public List<StageData> stageDatas;
    public PencilCaseData enemyPencilCase;
}

[System.Serializable]
public class StageData
{
    public string name;
    public float max_Range;
    public TimeType timeType;
    [Header("ActiveTime일 때만")]
    public float timeValue;
}