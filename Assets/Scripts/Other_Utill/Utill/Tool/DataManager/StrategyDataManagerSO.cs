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

		private static List<StrategyData> _stdStarategyList = new List<StrategyData>(); //Strategy데이터 리스트

		//디버그용
		[SerializeField]
		private StrategyServerData testList;
		[SerializeField]
		private List<StrategyData> _debugStdStarategyList;

		/// <summary>
		/// 기준 데이터에 입력 데이터를 넣는다
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<StrategyServerData>(SetStrategyList, ServerDataConnect.DataType.StrategyData);
		}

		/// <summary>
		/// 디버그용 초기화
		/// </summary>
		public void DebugInitialize()
		{
			_stdStarategyList = _debugStdStarategyList;
		}

		/// <summary>
		/// StrategyType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static StrategyData FindStrategyData(StrategyType strategyType)
		{
			StrategyData findData = null;
			findData = _stdStarategyList.Find(x => x._starategyType == strategyType);
			if(findData == null)
			{
				Debug.LogError($"{strategyType} : 전략 데이터가 존재하지 않음");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// 전략 기준데이터 설정
		/// </summary>
		/// <param name="strategyDatas"></param>
		public void SetStrategyList(StrategyServerData strategyDatas)
		{
			testList = strategyDatas;
			   _stdStarategyList = strategyDatas.post;
		}

	}

}