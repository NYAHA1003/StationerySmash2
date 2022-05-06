using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;
[CreateAssetMenu(fileName = "LoadingBattleDataSO", menuName = "Scriptable Object/Loading/LoadingBattleDataSO")]
public class LoadingBattleDataSO : ScriptableObject
{
    public List<LoadData> loadDatas = new List<LoadData>();
}
