using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Load;
using Utill.Tool;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Object/StageDataSO")]
public class StageDataListSO : ScriptableObject
{
    public List<StageData> stageDatas;
}

[System.Serializable]
public class StageData
{
    public string _stageName;
    public int _rewardExp;
    public int _rewardMoney;
    public BattleStageType stageType;
    public TimeType _timeType;
    public float max_Range;
    public float timeValue;
}