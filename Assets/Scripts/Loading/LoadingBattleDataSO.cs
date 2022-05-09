using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;
[CreateAssetMenu(fileName = "LoadingBattleDataSO", menuName = "Scriptable Object/Loading/LoadingBattleDataSO")]
public class LoadingBattleDataSO : ScriptableObject
{
	
    public List<LoadData> loadDatas = new List<LoadData>();
    public LoadData CurrentStageData
	{
		get
		{
			return loadDatas[_currentIndex];
		}
	}
	private int _currentIndex = 0;

	public void SetCurrentIndex(int index)
	{
		_currentIndex = index;
	}
}
