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
	[CreateAssetMenu(fileName = "BadgeDataManagerSO", menuName = "Scriptable Object/BadgeDataManagerSO")]
	public class BadgeDataManagerSO : ScriptableObject, Iinitialize
	{
		[System.Serializable]
		public class BadgeServerData
		{
			public List<BadgeData> post;
		}

		//프로퍼티
		public static List<BadgeData> HaveBadgeDataList => _haveBadgeDataList;

		//속성
		private static List<BadgeData> _stdBadgeDataList = new List<BadgeData>(); //Badge 기준 데이터 리스트
		private static List<BadgeData> _haveBadgeDataList = new List<BadgeData>(); //Badge 가진 데이터 리스트

		//디버그용

		[SerializeField]
		private BadgeServerData testList;

		[SerializeField]
		private List<BadgeData> _debugStdBadgeDataList;


		/// <summary>
		/// 서버 데이터를 받아 초기화
		/// </summary>
		/// <param name="serverDataConnect"></param>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<BadgeServerData>(SetUnitDataList, ServerDataConnect.DataType.UnitData);
		}

		/// <summary>
		/// 디버그용 초기화
		/// </summary>
		public void DebugInitialize()
		{
			_stdBadgeDataList = _debugStdBadgeDataList;
		}

		/// <summary>
		/// 뱃지 데이터 리스트 설정
		/// </summary>
		/// <param name="unitDatas"></param>
		public void SetUnitDataList(BadgeServerData badgeDatas)
		{
			testList = badgeDatas;
			   _stdBadgeDataList = badgeDatas.post;

			_haveBadgeDataList.Clear();

			int count = UserSaveManagerSO.UserSaveData._haveBadgeSaveDatas.Count;

			for (int i = 0; i < count; i++)
			{
				BadgeSaveData badgeSaveData = UserSaveManagerSO.UserSaveData._haveBadgeSaveDatas[i];
				_haveBadgeDataList.Add(_stdBadgeDataList.Find(x => x._badgeType == badgeSaveData._BadgeType).DeepCopyBadgeData(badgeSaveData));
			}
		}
	}
}
