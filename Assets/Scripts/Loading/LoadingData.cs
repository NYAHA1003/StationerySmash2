using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LoadData
{
    public PencilCaseData PencilCasedataBase;
    public int summonGrade;
    public float throwSpeed;
    public bool isAIOn;
    public List<CardData> cardDataList;
    public List<Vector2> pos;
    public List<float> max_Delay;
}
