using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Battle.Starategy;


namespace Utill.Tool
{
	[CreateAssetMenu(fileName = "UnitDataManagerSO", menuName = "Scriptable Object/UnitDataManagerSO")]
	public class UnitDataManagerSO : ScriptableObject, Iinitialize
	{
		[System.Serializable]
		public class UnitServerData
		{
			public List<UnitData> post;
		}

		private static List<UnitData> _stdUnitDataList = new List<UnitData>(); //Unit데이터 리스트

		[SerializeField]
		private UnitServerData testList;
		[SerializeField]
		private List<UnitData> _debugStdUnitDataList;

		/// <summary>
		/// 기준 데이터에 입력 데이터를 넣는다
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<UnitServerData>(SetUnitDataList, ServerDataConnect.DataType.UnitData);
		}

		/// <summary>
		/// 디버그용 초기화
		/// </summary>
		public void DebugInitialize()
		{
			_stdUnitDataList = _debugStdUnitDataList;
		}

		/// <summary>
		/// StrategyType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindUnitData(UnitType unitType)
		{
			UnitData findData = null;
			findData = _stdUnitDataList.Find(x => x._unitType == unitType);
			if (findData == null)
			{
				Debug.LogError($"{unitType} : 유닛 데이터가 존재하지 않음");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// 유닛 데이터 리스트 설정
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetUnitDataList(UnitServerData unitDatas)
		{
			testList = unitDatas;
			_stdUnitDataList = unitDatas.post;
		}
	}


}
