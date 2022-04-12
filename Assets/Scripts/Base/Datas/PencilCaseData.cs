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
    public AbstractPencilCaseAbility pencilState;
    public CardData pencilCaseData;
    public List<BadgeData> _badgeDatas;
}
