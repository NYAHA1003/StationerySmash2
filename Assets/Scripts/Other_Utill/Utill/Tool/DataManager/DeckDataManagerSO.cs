using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
	[CreateAssetMenu(fileName = "DeckDataManagerSO", menuName = "Scriptable Object/DeckDataManagerSO")]
	public class DeckDataManagerSO : ScriptableObject, Iinitialize
	{
		[System.Serializable]
		public class DeckServerData
		{
			public List<CardData> post;
		}
		
		//프로퍼티
		public static List<CardData> StdDeckDataList => _stdDeckDataList;

		private static List<CardData> _stdDeckDataList = new List<CardData>(); //Unit데이터 리스트

		public DeckServerData testList;

		/// <summary>
		/// 기준 데이터를 받아온다
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<DeckServerData>(SetDeckDataList, ServerDataConnect.DataType.DeckData);
		}

		/// <summary>
		/// 덱 데이터 리스트 설정
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetDeckDataList(DeckServerData deckDatas)
		{
			testList = deckDatas;
			_stdDeckDataList = deckDatas.post;
		}
	}
}
