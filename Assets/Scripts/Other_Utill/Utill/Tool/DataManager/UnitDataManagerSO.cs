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
		private static List<UnitData> _haveUnitDataList = new List<UnitData>(); //가지고 있는 Unit데이터 리스트

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
			SetHaveData();
		}

		/// <summary>
		/// StrategyType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindStdUnitData(UnitType unitType)
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
		/// StrategyType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindHaveUnitData(UnitType unitType)
		{
			UnitData findData = null;
			findData = _haveUnitDataList.Find(x => x._unitType == unitType);
			if (findData == null)
			{
				Debug.LogError($"{unitType} : 유닛 데이터가 존재하지 않음");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// 가진 유닛데이터 추가
		/// </summary>
		/// <param name="unitType"></param>
		public static void AddHaveData(UnitType unitType)
		{
			UnitData unitData = FindStdUnitData(unitType);
			_haveUnitDataList.Add(unitData);
		}

		/// <summary>
		/// 유닛 데이터 리스트 설정
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetUnitDataList(UnitServerData unitDatas)
		{
			testList = unitDatas;
			_stdUnitDataList = unitDatas.post;
			SetHaveData();
		}

		/// <summary>
		/// 가진 데이터 설정
		/// </summary>
		private void SetHaveData()
		{
			_haveUnitDataList.Clear();

			for (int i = 0; i < DeckDataManagerSO.HaveDeckDataList.Count; ++i)
			{
				UnitData unitData = FindStdUnitData(DeckDataManagerSO.HaveDeckDataList[i]._unitType);
				int count = 1;
				int level = DeckDataManagerSO.HaveDeckDataList[i]._level;
				UnitData haveUnitData = new UnitData()
				{
					_accuracy = unitData._accuracy,
					_hp = unitData._hp + (unitData._hp * (level - 1) / 10),
					_weight = unitData._weight,
					_knockback = unitData._knockback,
					_dir = unitData._dir,
					_moveSpeed = unitData._moveSpeed,
					_damage = unitData._damage + (unitData._damage / 10 * (level - 1)),
					_attackSpeed = unitData._attackSpeed,
					_range = unitData._range,
					_colideData = unitData._colideData,
					_stickerType = unitData._stickerType,
					_attackType = unitData._attackType,
					_unitType = unitData._unitType,
					_unitablityData = unitData._unitablityData,
				};

				_haveUnitDataList.Add(haveUnitData);
			}
		}

	}


}
