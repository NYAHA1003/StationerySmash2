using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDeckData : MonoBehaviour
{
    public SaveDataSO saveData;
    private CardDeckSO cardDeck;

    string json; 
    /// <summary>
    /// 유저 세이브 데이터를 Json화 시켜 저장한다
    /// </summary>
    [ContextMenu("DataToJson")]
    public void DataToJson()    
    {
         json = JsonUtility.ToJson(saveData.userSaveData, false);
        //json을 저장한다 파일로 유니티에셋파일쪽에
        Debug.Log(json);
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json); 
    }


    /// <summary>
    /// json화 시킨 
    /// </summary>
    [ContextMenu("JsonToData")]
    public void JsonToData()
    {
        saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(json);
    }
}
