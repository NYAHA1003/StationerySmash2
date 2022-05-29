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
		
		//������Ƽ
		public static List<CardData> StdDeckDataList => _stdDeckDataList;

		private static List<CardData> _stdDeckDataList = new List<CardData>(); //Unit������ ����Ʈ

		public DeckServerData testList;

		/// <summary>
		/// ���� �����͸� �޾ƿ´�
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<DeckServerData>(SetDeckDataList, ServerDataConnect.DataType.DeckData);
		}

		/// <summary>
		/// �� ������ ����Ʈ ����
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetDeckDataList(DeckServerData deckDatas)
		{
			testList = deckDatas;
			_stdDeckDataList = deckDatas.post;
		}
	}
}
