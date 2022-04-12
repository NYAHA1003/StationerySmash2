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
    /// ���� ���̺� �����͸� Jsonȭ ���� �����Ѵ�
    /// </summary>
    [ContextMenu("DataToJson")]
    public void DataToJson()    
    {
         json = JsonUtility.ToJson(saveData.userSaveData, false);
        //json�� �����Ѵ� ���Ϸ� ����Ƽ���������ʿ�
        Debug.Log(json);
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json); 
    }


    /// <summary>
    /// jsonȭ ��Ų 
    /// </summary>
    [ContextMenu("JsonToData")]
    public void JsonToData()
    {
        saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(json);
    }
}
