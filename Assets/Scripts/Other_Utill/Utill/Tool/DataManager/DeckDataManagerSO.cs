using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Main.Deck;
using Utill.Data;
using Utill.Tool;

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
		public static List<CardData> HaveDeckDataList => _haveDeckList;

		private static List<CardData> _stdDeckDataList = new List<CardData>(); //기준 카드 데이터 리스트
		private static List<CardData> _haveDeckList = new List<CardData>(); //보유 카드 데이터 리스트

		/// <summary>
		/// 디버그용
		/// </summary>
		[SerializeField]
		private DeckServerData testList;
		[SerializeField]
		private List<CardData> _debugStdDeckDataList;

		/// <summary>
		/// 기준 데이터를 받아온다
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<DeckServerData>(SetDeckDataList, ServerDataConnect.DataType.DeckData);
		}

		/// <summary>
		/// 디버그용 데이터 초기화
		/// </summary>
		public void DebugInitialize()
		{
			_stdDeckDataList = _debugStdDeckDataList;
			SetHaveDataList();
		}

		/// <summary>
		/// 카드 데이터 찾기
		/// </summary>
		public static CardData FindStdCardData(CardNamingType cardNamingType)
		{
			return _stdDeckDataList.Find(x => x._cardNamingType == cardNamingType);
		}


		/// <summary>
		/// 가진 카드 데이터 찾기
		/// </summary>
		public static CardData FindHaveCardData(CardNamingType cardNamingType)
		{
			return _haveDeckList.Find(x => x._cardNamingType == cardNamingType);
		}

		/// <summary>
		/// 덱 데이터 리스트 설정
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetDeckDataList(DeckServerData deckDatas)
		{
			testList = deckDatas;
			_stdDeckDataList = deckDatas.post;
			SetHaveDataList();
		}

		/// <summary>
		/// 가진 카드 데이터 설정
		/// </summary>
		public static void SetHaveDataList()
		{
			_haveDeckList.Clear();
			int count = UserSaveManagerSO.UserSaveData._haveCardSaveDatas.Count;
			for (int i = 0; i < count; ++i)
			{
				CardData cardData = FindStdCardData(UserSaveManagerSO.UserSaveData._haveCardSaveDatas[i]._cardNamingType).DeepCopy();
				cardData._level = UserSaveManagerSO.UserSaveData._haveCardSaveDatas[i]._level;
				cardData._count = UserSaveManagerSO.UserSaveData._haveCardSaveDatas[i]._count;
				_haveDeckList.Add(cardData);
			}
		}

		/// <summary>
		/// 이름순으로 가진 카드 정렬
		/// </summary>
		public static void HaveCardSortABC()
		{
			var list = _haveDeckList.OrderBy(x => x._name);
			_haveDeckList = list.ToList<CardData>();
		}

		/// <summary>
		/// 코스트순으로 가진 카드 정렬
		/// </summary>
		public static void HaveCardSortCost()
		{
			var list = _haveDeckList.OrderBy(x => x._cost);
			_haveDeckList = list.ToList<CardData>();
		}
	}
}
