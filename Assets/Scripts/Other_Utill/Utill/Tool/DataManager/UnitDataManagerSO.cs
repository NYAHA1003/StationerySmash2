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
		}

		/// <summary>
		/// StrategyType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindUnitData(UnitType unitType)
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
		/// ���� ������ ����Ʈ ����
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetUnitDataList(UnitServerData unitDatas)
		{
			testList = unitDatas;
			_stdUnitDataList = unitDatas.post;
		}
	}


}
