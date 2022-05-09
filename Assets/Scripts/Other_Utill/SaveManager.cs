using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Main.Deck;
using Utill.Data;

public class SaveManager : MonoSingleton<SaveManager>
{ 
    public SaveDataSO SaveData
	{
        get
		{
            if(_saveData == null)
			{
                throw new System.Exception("���̺� �Ŵ����� �̻���");
			}
            return _saveData;
		}
	}
    [SerializeField]
    private SaveDataSO _saveData;
    private bool isLoadData = false;
    public bool IsLoadData
	{
        get
		{
            return isLoadData;
		}
	}

	private void Awake()
	{
        if(!_instance.isLoadData)
		{
            isLoadData = true;
            LoadJsonData();
		}
	}

	private void Start()
	{
        DeliverDataToObserver();
    }

	/// <summary>
	/// ����� Json �����͸� �ҷ�����
	/// </summary>
	public void LoadJsonData()
    {
        //json ���� �ҷ�����
        try
        {
            string path = File.ReadAllText(Application.dataPath + "/saveData.json");


            //���������Ϳ� ����
            _saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
            DeliverDataToObserver();
        }
        catch
        {
            //���� �߸� ������ ����
        }
    }

    /// <summary>
    /// ���� ���̺� �����͸� Jsonȭ ���� �����Ѵ�
    /// </summary>
    public void SaveJsonData()
    {
        //�����͸� json���� ��ȯ
        string json = JsonUtility.ToJson(_saveData.userSaveData, true);

        //json�� �����Ѵ� ���Ϸ� ����Ƽ���������ʿ�
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json);
        DeliverDataToObserver();
    }

    /// <summary>
    /// ��� �����ڵ鿡�� ������ ����
    /// </summary>
    public void DeliverDataToObserver()
    {
        _saveData.DeliverDataToObserver();
    }


    /// <summary>
    /// ����ġ ����
    /// </summary>
    public void AddExp(int exp)
	{
        _instance.SaveData.userSaveData.AddExp(exp);
        DeliverDataToObserver();
    }
}
