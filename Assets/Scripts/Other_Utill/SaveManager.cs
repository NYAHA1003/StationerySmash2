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
        Instance.SaveData.userSaveData.AddExp(exp);
        DeliverDataToObserver();
    }

    /// <summary>
    /// �� ����
    /// </summary>
    public void AddMoney(int money)
    {
        Instance.SaveData.userSaveData.AddMoney(money);
        DeliverDataToObserver();
    }
}
