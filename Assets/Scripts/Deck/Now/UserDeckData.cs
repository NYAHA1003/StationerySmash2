using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utill; 

public class UserDeckData : MonoBehaviour
{
    public SaveDataSO saveData; //���� ������(ī�� ����, ��������) 
    public CardDeckSO cardDeck; //���� ������ 

    public GameObject cardPrefab; 
    
    /// <summary>
    /// ī�嵦 �����͸� �������ݴϴ� 
    /// </summary>
    public void SetCardData()
    {
        JsonToData();
        for(int i = 0; i < saveData.userSaveData.unitSaveDatas.Count; i++)
        {

        }
        //cardDeck.cardDatas
    }

    /// <summary>
    /// ���� ���̺� �����͸� Jsonȭ ���� �����Ѵ�
    /// </summary>
    [ContextMenu("DataToJson")]
    public void DataToJson()    
    {
         string json = JsonUtility.ToJson(saveData.userSaveData, true);
        //json�� �����Ѵ� ���Ϸ� ����Ƽ���������ʿ�
        Debug.Log(json);
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        Debug.Log(path); 
        File.WriteAllText(path, json); 
    }
    /// <summary>
    /// jsonȭ ��Ų �����͸� �����ͷ�  �����´�
    /// </summary>
    [ContextMenu("JsonToData")]
    public void JsonToData()
    {
        string path = File.ReadAllText(Application.dataPath + "/saveData.json");
        Debug.Log(path);    
        saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
    }
}
