using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Main.Deck;
using Utill.Data;

public class SaveManager : MonoSingleton<SaveManager>
{ 
    public SaveDataSO _saveData;

    /// <summary>
    /// ����� Json �����͸� �ҷ�����
    /// </summary>
    public void LoadData()
    {
        //json ���� �ҷ�����
        try
        {
            string path = File.ReadAllText(Application.dataPath + "/saveData.json");


            //���������Ϳ� ����
            _saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
        }
        catch
        {
            //���� �߸� ������ ����
        }
    }

    /// <summary>
    /// ���� ���̺� �����͸� Jsonȭ ���� �����Ѵ�
    /// </summary>
    public void SaveData()
    {
        //�����͸� json���� ��ȯ
        string json = JsonUtility.ToJson(_saveData.userSaveData, true);

        //json�� �����Ѵ� ���Ϸ� ����Ƽ���������ʿ�
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json);
    }
}
