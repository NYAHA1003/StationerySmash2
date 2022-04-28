using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle.PCAbility;

[System.Serializable]
public class PencilCaseData
{
    public int maxCard;
    public float costSpeed;
    public float throwGaugeSpeed;
    public PencilCaseType pencilCaseType;
    public AbstractPencilCaseAbility pencilState;
    public CardData pencilCaseData;
    public List<BadgeData> _badgeDatas;
}
