using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Main.Deck;
using Battle.Starategy;


namespace Utill.Tool
{
	[CreateAssetMenu(fileName = "StickerDataManagerSO", menuName = "Scriptable Object/StickerDataManagerSO")]
	public class StickerDataManagerSO : ScriptableObject, Iinitialize
	{
		[System.Serializable]
		public class StickerServerData
		{
			public List<StickerData> post;
		}

		private static List<StickerData> _stdStickerDataList = new List<StickerData>(); //Unit������ ����Ʈ
		private static List<StickerData> _haveStickerDataList = new List<StickerData>(); //Unit������ ����Ʈ

		public StickerServerData testList;

		/// <summary>
		/// ���� �����Ϳ� �Է� �����͸� �ִ´�
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<StickerServerData>(SetStickerDataList, ServerDataConnect.DataType.StickerData);
		}

		/// <summary>
		/// stickerType�� �´� Strategy�����͸� ��ȯ��
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static StickerData FindStickerData(StickerType stickerType)
		{
			StickerData findData = null;
			findData = _stdStickerDataList.Find(x => x._stickerType == stickerType);
			if (findData == null)
			{
				Debug.LogError($"{stickerType} : ���� �����Ͱ� �������� ����");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// ���� ���� Ÿ���� None�� ��ƼĿ �����͵��� ��ȯ
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static List<StickerData> FindStickerDataNoneTypeList()
		{
			var findData = _stdStickerDataList.FindAll(x => x._onlyUnitType == UnitType.None);
			if (findData == null)
			{
				return null;
			}
			return findData;
		}

		/// <summary>
		/// ���� ���� Ÿ�Կ� �´� ��ƼĿ �����͵��� ��ȯ
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static List<StickerData> FindStickerDataOnlyUnitTypeList(UnitType unitType)
		{
			var findData = _stdStickerDataList.FindAll(x => x._onlyUnitType == unitType);
			if (findData == null)
			{
				return null;
			}
			return findData;
		}

		/// <summary>
		/// ���� ��ƼĿ �����͸� �����Ѵ�
		/// </summary>
		public void SetStickerDataList(StickerServerData stickerDatas)
		{
			testList = stickerDatas;

			_stdStickerDataList = stickerDatas.post;
			_haveStickerDataList.Clear();

			int count = UserSaveManagerSO.UserSaveData._haveStickerList.Count;

			//������ �ִ� ��ƼĿ �˸´� ��ƼĿ ������ ã��
			for (int i = 0; i < count; i++)
			{
				StickerSaveData stickerSaveData = UserSaveManagerSO.UserSaveData._haveStickerList[i];
				StickerData addStickerData = _stdStickerDataList.Find(x => x._stickerType == stickerSaveData._stickerType);
				if (addStickerData != null)
				{
					StickerData stickerData = addStickerData.DeepCopy();
					stickerData._level = stickerSaveData._level;
					_haveStickerDataList.Add(stickerData);
				}
			}
		}
	}
}
