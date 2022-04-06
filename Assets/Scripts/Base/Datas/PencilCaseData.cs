using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class PencilCaseData
{
    public int maxCard;
    public float costSpeed;
    public PencilCaseType pencilCaseType;
    public AbstractPencilCaseAbilityState pencilState;
    public DataBase pencilCaseData;
}
