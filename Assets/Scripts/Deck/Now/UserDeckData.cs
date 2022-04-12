using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDeckData : MonoBehaviour
{
    public SaveDataSO saveData;
    public CardDeckSO cardDeck;

    /// <summary>
    /// 유저 세이브 데이터를 Json화 시켜 저장한다
    /// </summary>
    [ContextMenu("DataToJson")]
    public void DataToJson()    
    {
         string json = JsonUtility.ToJson(saveData.userSaveData, true);
        //json을 저장한다 파일로 유니티에셋파일쪽에
        Debug.Log(json);
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        Debug.Log(path); 
        File.WriteAllText(path, json); 
    }


    /// <summary>
    /// json화 시킨 데이터를 데이터로  가져온다
    /// </summary>
    [ContextMenu("JsonToData")]
    public void JsonToData()
    {
        string path = File.ReadAllText(Application.dataPath + "/saveData.json");
        Debug.Log(path);    
        saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
    }
}
