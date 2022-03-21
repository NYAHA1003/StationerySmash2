using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIDataSO", menuName = "Scriptable Object/AIDataSO")]
public class AIDataSO : ScriptableObject
{
    public int summonGrade;
    public float throwSpeed;
    public List<DataBase> cardDataList;
    public List<Vector2> pos;
    public List<float> max_Delay;
}
