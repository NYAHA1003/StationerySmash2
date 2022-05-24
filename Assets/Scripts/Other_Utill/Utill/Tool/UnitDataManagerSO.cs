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
		private static List<UnitData> _stdUnitDataList = new List<UnitData>(); //Unit������ ����Ʈ
		public List<UnitData> _inputUnitDataList = new List<UnitData>(); //Unit������ ����Ʈ


		/// <summary>
		/// ���� �����Ϳ� �Է� �����͸� �ִ´�
		/// </summary>
		public void Reset()
		{
			_stdUnitDataList = _inputUnitDataList;
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
	}
}
