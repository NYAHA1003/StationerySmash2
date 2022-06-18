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

		//������Ƽ
		public static List<CardData> StdDeckDataList => _stdDeckDataList;
		public static List<CardData> HaveDeckDataList => _haveDeckList;

		private static List<CardData> _stdDeckDataList = new List<CardData>(); //���� ī�� ������ ����Ʈ
		private static List<CardData> _haveDeckList = new List<CardData>(); //���� ī�� ������ ����Ʈ

		/// <summary>
		/// ����׿�
		/// </summary>
		[SerializeField]
		private DeckServerData testList;
		[SerializeField]
		private List<CardData> _debugStdDeckDataList;

		/// <summary>
		/// ���� �����͸� �޾ƿ´�
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<DeckServerData>(SetDeckDataList, ServerDataConnect.DataType.DeckData);
		}

		/// <summary>
		/// ����׿� ������ �ʱ�ȭ
		/// </summary>
		public void DebugInitialize()
		{
			_stdDeckDataList = _debugStdDeckDataList;
			SetHaveDataList();
		}

		/// <summary>
		/// ī�� ������ ã��
		/// </summary>
		public static CardData FindStdCardData(CardNamingType cardNamingType)
		{
			return _stdDeckDataList.Find(x => x._cardNamingType == cardNamingType);
		}


		/// <summary>
		/// ���� ī�� ������ ã��
		/// </summary>
		public static CardData FindHaveCardData(CardNamingType cardNamingType)
		{
			return _haveDeckList.Find(x => x._cardNamingType == cardNamingType);
		}

		/// <summary>
		/// �� ������ ����Ʈ ����
		/// </summary>
		/// <param name="unitDatas"></param>
		private void SetDeckDataList(DeckServerData deckDatas)
		{
			testList = deckDatas;
			_stdDeckDataList = deckDatas.post;
			SetHaveDataList();
		}

		/// <summary>
		/// ���� ī�� ������ ����
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
		/// �̸������� ���� ī�� ����
		/// </summary>
		public static void HaveCardSortABC()
		{
			var list = _haveDeckList.OrderBy(x => x._name);
			_haveDeckList = list.ToList<CardData>();
		}

		/// <summary>
		/// �ڽ�Ʈ������ ���� ī�� ����
		/// </summary>
		public static void HaveCardSortCost()
		{
			var list = _haveDeckList.OrderBy(x => x._cost);
			_haveDeckList = list.ToList<CardData>();
		}
	}
}
