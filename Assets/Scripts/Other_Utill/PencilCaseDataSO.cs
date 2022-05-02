using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PencilCaseDataSO", menuName = "Scriptable Object/PencilCaseDataSO")]
public class PencilCaseDataSO : ScriptableObject
{
    public PencilCaseData PencilCasedataBase;

    public object PencilCaseData { get; internal set; }
}
