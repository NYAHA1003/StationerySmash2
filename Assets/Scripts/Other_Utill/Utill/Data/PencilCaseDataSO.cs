using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;
using Utill.Data;
[CreateAssetMenu(fileName = "PencilCaseDataSO", menuName = "Scriptable Object/PencilCaseDataSO")]

public class PencilCaseDataSO : ScriptableObject
{
    public PencilCaseData _pencilCaseData;
    public BattleStageType battleStageType;
}
