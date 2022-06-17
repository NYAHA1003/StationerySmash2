using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Load;
using Utill.Tool;



[CreateAssetMenu(fileName = "CurrentStageDataSO", menuName = "Scriptable Object/CurrentStageDataSO")]
public class CurrentStageData : ScriptableObject
{
    public StageData _currentStageDatas;
    //
}