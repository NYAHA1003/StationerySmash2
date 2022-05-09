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
                throw new System.Exception("세이브 매니저가 이상함");
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
	/// 저장된 Json 데이터를 불러오기
	/// </summary>
	public void LoadJsonData()
    {
        //json 파일 불러오기
        try
        {
            string path = File.ReadAllText(Application.dataPath + "/saveData.json");


            //유저데이터에 저장
            _saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
            DeliverDataToObserver();
        }
        catch
        {
            //에러 뜨면 파일이 없음
        }
    }

    /// <summary>
    /// 유저 세이브 데이터를 Json화 시켜 저장한다
    /// </summary>
    public void SaveJsonData()
    {
        //데이터를 json으로 변환
        string json = JsonUtility.ToJson(_saveData.userSaveData, true);

        //json을 저장한다 파일로 유니티에셋파일쪽에
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json);
        DeliverDataToObserver();
    }

    /// <summary>
    /// 모든 관찰자들에게 데이터 전달
    /// </summary>
    public void DeliverDataToObserver()
    {
        _saveData.DeliverDataToObserver();
    }


    /// <summary>
    /// 경험치 증가
    /// </summary>
    public void AddExp(int exp)
	{
        _instance.SaveData.userSaveData.AddExp(exp);
        DeliverDataToObserver();
    }
}
