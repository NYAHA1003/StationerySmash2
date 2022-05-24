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
	public class UnitDataManagerSO : ScriptableObject
	{
		private static List<UnitData> _stdUnitDataList = new List<UnitData>(); //Unit데이터 리스트
		public List<UnitData> _inputUnitDataList = new List<UnitData>(); //Unit데이터 리스트


		/// <summary>
		/// 기준 데이터에 입력 데이터를 넣는다
		/// </summary>
		public void Reset()
		{
			_stdUnitDataList = _inputUnitDataList;
		}


		/// <summary>
		/// StrategyType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindUnitData(UnitType unitType)
		{
			UnitData findData = null;
			findData = _stdUnitDataList.Find(x => x.unitType == unitType);
			if (findData == null)
			{
				Debug.LogError($"{unitType} : 유닛 데이터가 존재하지 않음");
				return null;
			}
			return findData;
		}
	}
}
