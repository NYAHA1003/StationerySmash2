using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[CreateAssetMenu(fileName = "PencilCaseDataSO", menuName = "Scriptable Object/PencilCaseDataSO")]
public class PencilCaseDataSO : ScriptableObject
{
    public DataBase dataBase;
    public PencilCaseType pencilCaseType;
    public float costSpeed;
}
