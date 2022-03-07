using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[CreateAssetMenu(fileName = "PencilCaseDataSO", menuName = "Scriptable Object/PencilCaseDataSO")]
public class PencilCaseDataSO : ScriptableObject
{
    public PencilCaseType pencilCaseType;
    public DataBase data;
}
