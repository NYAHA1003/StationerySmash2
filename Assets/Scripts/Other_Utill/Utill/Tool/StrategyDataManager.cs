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
	public class StrategyDataManager
	{
		private static List<StrategyData> _stdStarategyList = new List<StrategyData>(); //Strategy데이터 리스트
		

		/// <summary>
		/// StrategyType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static StrategyData FindStrategyData(StrategyType strategyType)
		{
			StrategyData findData = null;
			findData = _stdStarategyList.Find(x => x.starategyType == strategyType);
			if(findData == null)
			{
				Debug.LogError($"{strategyType} : 전략 데이터가 존재하지 않음");
				return null;
			}
			return findData;
		}
	}

}