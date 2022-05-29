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
        if(!Instance.isLoadData)
		{
            isLoadData = true;
		}
	}

	private void Start()
	{
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
        Instance.SaveData.userSaveData.AddExp(exp);
        DeliverDataToObserver();
    }

    /// <summary>
    /// 돈 증가
    /// </summary>
    public void AddMoney(int money)
    {
        Instance.SaveData.userSaveData.AddMoney(money);
        DeliverDataToObserver();
    }
}
