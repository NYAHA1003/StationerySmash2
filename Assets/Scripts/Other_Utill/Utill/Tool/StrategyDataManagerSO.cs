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
	[CreateAssetMenu(fileName = "StrategyDataManagerSO", menuName = "Scriptable Object/StrategyDataManagerSO")]
	public class StrategyDataManagerSO : ScriptableObject, Iinitialize 
	{
		private static List<StrategyData> _stdStarategyList = new List<StrategyData>(); //Strategy������ ����Ʈ
		public List<StrategyData> _inputStarategyList = new List<StrategyData>(); //Strategy������ ����Ʈ

		/// <summary>
		/// ���� �����Ϳ� �Է� �����͸� �ִ´�
		/// </summary>
		public void Initialize()
		{ 
			_stdStarategyList = _inputStarategyList;
		}

		/// <summary>
		/// StrategyType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static StrategyData FindStrategyData(StrategyType strategyType)
		{
			StrategyData findData = null;
			findData = _stdStarategyList.Find(x => x.starategyType == strategyType);
			if(findData == null)
			{
				Debug.LogError($"{strategyType} : ���� �����Ͱ� �������� ����");
				return null;
			}
			return findData;
		}

	}

}