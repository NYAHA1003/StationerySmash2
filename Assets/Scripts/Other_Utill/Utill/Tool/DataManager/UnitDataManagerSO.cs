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

		private static List<UnitData> _stdUnitDataList = new List<UnitData>(); //Unit������ ����Ʈ
		private static List<UnitData> _haveUnitDataList = new List<UnitData>(); //������ �ִ� Unit������ ����Ʈ

		[SerializeField]
		private UnitServerData testList;
		[SerializeField]
		private List<UnitData> _debugStdUnitDataList;

		/// <summary>
		/// ���� �����Ϳ� �Է� �����͸� �ִ´�
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<UnitServerData>(SetUnitDataList, ServerDataConnect.DataType.UnitData);
		}

		/// <summary>
		/// ����׿� �ʱ�ȭ
		/// </summary>
		public void DebugInitialize()
		{
			_stdUnitDataList = _debugStdUnitDataList;
			SetHaveData();
		}

		/// <summary>
		/// StrategyType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindStdUnitData(UnitType unitType)
		{
			UnitData findData = null;
			findData = _stdUnitDataList.Find(x => x._unitType == unitType);
			if (findData == null)
			{
				Debug.LogError($"{unitType} : ���� �����Ͱ� �������� ����");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// StrategyType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindHaveUnitData(UnitType unitType)
		{
			UnitData findData = null;
			findData = _haveUnitDataList.Find(x => x._unitType == unitType);
			if (findData == null)
			{
				Debug.LogError($"{unitType} : ���� �����Ͱ� �������� ����");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// ���� ���ֵ����� �߰�
		/// </summary>
		/// <param name="unitType"></param>
		public static void AddHaveData(UnitType unitType)
		{
			UnitData unitData = FindStdUnitData(unitType);
			_haveUnitDataList.Add(unitData);
		}

		/// <summary>
		/// ���� ������ ����Ʈ ����
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetUnitDataList(UnitServerData unitDatas)
		{
			testList = unitDatas;
			_stdUnitDataList = unitDatas.post;
			SetHaveData();
		}

		/// <summary>
		/// ���� ������ ����
		/// </summary>
		private void SetHaveData()
		{
			_haveUnitDataList.Clear();

			for (int i = 0; i < DeckDataManagerSO.HaveDeckDataList.Count; ++i)
			{
				UnitData unitData = FindStdUnitData(DeckDataManagerSO.HaveDeckDataList[i]._unitType);
				int count = 1;
				int level = DeckDataManagerSO.HaveDeckDataList[i]._level;
				while (count < level)
				{
					unitData._hp += unitData._hp * count / 10;
					unitData._damage += unitData._damage / 10 * count;
					count++;
				}

				_haveUnitDataList.Add(unitData);
			}
		}

	}


}
