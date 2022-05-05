using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;

[CreateAssetMenu(fileName = "PencilCaseDataSO", menuName = "Scriptable Object/PencilCaseDataSO")]
public class PencilCaseDataSO : ScriptableObject
{
    public PencilCaseData _pencilCasedata;
    public BattleStageType battleStageType;
}
