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
		private static List<StickerData> _stdStickerDataList = new List<StickerData>(); //Unit데이터 리스트
		private static List<StickerData> _haveStickerDataList = new List<StickerData>(); //Unit데이터 리스트

		/// <summary>
		/// 기준 데이터에 입력 데이터를 넣는다
		/// </summary>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<List<StickerData>>(SetStickerDataList, ServerDataConnect.DataType.StickerData);
		}

		/// <summary>
		/// stickerType에 맞는 Strategy데이터를 반환함
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static StickerData FindStickerData(StickerType stickerType)
		{
			StickerData findData = null;
			findData = _stdStickerDataList.Find(x => x.StickerType == stickerType);
			if (findData == null)
			{
				Debug.LogError($"{stickerType} : 유닛 데이터가 존재하지 않음");
				return null;
			}
			return findData;
		}

		/// <summary>
		/// 제한 유닛 타입이 None인 스티커 데이터들을 반환
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static List<StickerData> FindStickerDataNoneTypeList()
		{
			var findData = _stdStickerDataList.FindAll(x => x.OnlyUnitType == UnitType.None);
			if (findData == null)
			{
				return null;
			}
			return findData;
		}

		/// <summary>
		/// 제한 유닛 타입에 맞는 스티커 데이터들을 반환
		/// </summary>
		/// <param name="strategyType"></param>
		/// <returns></returns>
		public static List<StickerData> FindStickerDataOnlyUnitTypeList(UnitType unitType)
		{
			var findData = _stdStickerDataList.FindAll(x => x.OnlyUnitType == unitType);
			if (findData == null)
			{
				return null;
			}
			return findData;
		}


		/// <summary>
		/// 보유 스티커 데이터를 설정한다
		/// </summary>
		public void SetStickerDataList(List<StickerData> stickerDatas)
		{
			_stdStickerDataList = stickerDatas;
			_haveStickerDataList.Clear();

			int count = UserSaveManagerSO.UserSaveData._haveStickerList.Count;

			//가지고 있는 스티커 알맞는 스티커 데이터 찾기
			for (int i = 0; i < count; i++)
			{
				StickerSaveData stickerSaveData = UserSaveManagerSO.UserSaveData._haveStickerList[i];
				StickerData addStickerData = _stdStickerDataList.Find(x => x.StickerType == stickerSaveData._stickerType);
				if (addStickerData != null)
				{
					StickerData stickerData = addStickerData.DeepCopy();
					stickerData.Level = stickerSaveData._level;
					_haveStickerDataList.Add(stickerData);
				}
			}
		}
	}
}
