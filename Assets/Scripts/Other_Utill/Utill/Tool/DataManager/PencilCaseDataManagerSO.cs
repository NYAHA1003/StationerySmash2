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
	[CreateAssetMenu(fileName = "PencilCaseDataManagerSO", menuName = "Scriptable Object/PencilCaseDataManagerSO")]
	public class PencilCaseDataManagerSO : ScriptableObject, Iinitialize
	{
		[System.Serializable]
		public class PencilCaseServerData
		{
			public List<PencilCaseData> post;
		}


		private static List<PencilCaseData> _stdPencilCaseDataList = new List<PencilCaseData>();
		private static List<PencilCaseData> _havePencilCaseDataList = new List<PencilCaseData>();
		private static PencilCaseData _inGamePencilCaseData = null;

		public static List<PencilCaseData> HavePencilCaseDataList => _havePencilCaseDataList;


		/// <summary>
		/// ���� �����͸� �޾� �ʱ�ȭ
		/// </summary>
		/// <param name="serverDataConnect"></param>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<PencilCaseServerData>(SetPencilCaseList, ServerDataConnect.DataType.PencilCaseData);
		}

		/// <summary>
		/// ���� ���� ����
		/// </summary>
		public static void SetPencilCaseList(PencilCaseServerData pencilCaseDatas)
		{
			_stdPencilCaseDataList = pencilCaseDatas.post;
			_havePencilCaseDataList.Clear();

			int count = UserSaveManagerSO.UserSaveData._havePencilCaseList.Count;
			
			for (int i = 0; i < count; i++)
			{
				PencilCaseType pcType = UserSaveManagerSO.UserSaveData._havePencilCaseList[i];

				PencilCaseData pcData = _stdPencilCaseDataList.Find(x => x._pencilCaseType == pcType);

				if (pcData != null)
				{
					_havePencilCaseDataList.Add(pcData);
				}
			}
		}

		/// <summary>
		/// ���� ���� ���� ���ΰ�ħ
		/// </summary>
		public static void ResetPencilCaseList()
		{
			_havePencilCaseDataList.Clear();

			int count = UserSaveManagerSO.UserSaveData._havePencilCaseList.Count;

			for (int i = 0; i < count; i++)
			{
				PencilCaseType pcType = UserSaveManagerSO.UserSaveData._havePencilCaseList[i];

				PencilCaseData pcData = _stdPencilCaseDataList.Find(x => x._pencilCaseType == pcType);

				if (pcData != null)
				{
					_havePencilCaseDataList.Add(pcData);
				}
			}
		}

		/// <summary>
		/// �ΰ��� ���� ����
		/// </summary>
		public static void SetInGamePencilCase(PencilCaseData pencilCaseData)
		{
			_inGamePencilCaseData = pencilCaseData;
		}
	}
}