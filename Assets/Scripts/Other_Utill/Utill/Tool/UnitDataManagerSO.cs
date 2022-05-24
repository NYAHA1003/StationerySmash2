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
	public class UnitDataManagerSO : ScriptableObject, IServerInitialize
	{
		private static List<UnitData> _stdUnitDataList = new List<UnitData>(); //Unit������ ����Ʈ

		/// <summary>
		/// ���� �����Ϳ� �Է� �����͸� �ִ´�
		/// </summary>
		public void ServerInitialize(ServerDataConnect serverDataConnect)
		{
			serverDataConnect.GetStandardUnitData(SetUnitDataList);
		}

		/// <summary>
		/// StrategyType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static UnitData FindUnitData(UnitType unitType)
		{
			UnitData findData = null;
			findData = _stdUnitDataList.Find(x => x.unitType == unitType);
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
		public void SetUnitDataList(List<UnitData> unitDatas)
		{
			_stdUnitDataList = unitDatas;
		}
	}
}
