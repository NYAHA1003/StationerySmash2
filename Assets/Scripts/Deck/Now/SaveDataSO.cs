using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Object/SaveDataSO")]
public class SaveDataSO : ScriptableObject
{
    //저장데이터(레벨, 가지고있는지) 저장
    public UserSaveData userSaveData; 
}
