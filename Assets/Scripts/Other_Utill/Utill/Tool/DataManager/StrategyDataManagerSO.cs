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
		[System.Serializable]
		public class StrategyServerData
		{
			public List<StrategyData> post;
		}

		private static List<StrategyData> _stdStarategyList = new List<StrategyData>(); //Strategy������ ����Ʈ

		//����׿�
		[SerializeField]
		private StrategyServerData testList;
		[SerializeField]
		private List<StrategyData> _debugStdStarategyList;

		/// <summary>
		/// ���� �����Ϳ� �Է� �����͸� �ִ´�
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<StrategyServerData>(SetStrategyList, ServerDataConnect.DataType.StrategyData);
		}

		/// <summary>
		/// ����׿� �ʱ�ȭ
		/// </summary>
		public void DebugInitialize()
		{
			_stdStarategyList = _debugStdStarategyList;
		}

		/// <summary>
		/// StrategyType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static StrategyData FindStrategyData(StrategyType strategyType)
		{
			StrategyData findData = null;
			findData = _stdStarategyList.Find(x => x._starategyType == strategyType);
			if(findData == null)
			{
				Debug.LogError($"{strategyType} : ���� �����Ͱ� �������� ����");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// ���� ���ص����� ����
		/// </summary>
		/// <param name="strategyDatas"></param>
		public void SetStrategyList(StrategyServerData strategyDatas)
		{
			testList = strategyDatas;
			   _stdStarategyList = strategyDatas.post;
		}

	}

}