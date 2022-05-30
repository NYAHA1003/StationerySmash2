using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Load;
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

		//������Ƽ
		public static PencilCaseData InGamePencilCaseData => _inGamePencilCaseData;
		public static PencilCaseData EnemyGamePencilCaseData => _enemyGamePencilCaseData;

		//�Ӽ�
		private static List<PencilCaseData> _stdPencilCaseDataList = new List<PencilCaseData>();
		private static List<PencilCaseData> _havePencilCaseDataList = new List<PencilCaseData>();
		private static PencilCaseData _inGamePencilCaseData = null;
		private static PencilCaseData _enemyGamePencilCaseData = new PencilCaseData();

		public static List<PencilCaseData> HavePencilCaseDataList => _havePencilCaseDataList;

		public PencilCaseServerData testList;

		/// <summary>
		/// ���� �����͸� �޾� �ʱ�ȭ
		/// </summary>
		/// <param name="serverDataConnect"></param>
		public void Initialize()
		{
			ServerDataConnect.Instance.GetData<PencilCaseServerData>(SetPencilCaseList, ServerDataConnect.DataType.PencilCaseData);
		}

		/// <summary>
		/// �� ���� ������ ����
		/// </summary>
		/// <param name="pencilCaseData"></param>
		public static void SetEnemyPencilCaseData(LoadData loadData)
		{
			_enemyGamePencilCaseData = loadData._pencilCaseData;
		}

		/// <summary>
		/// ���� ���� ����
		/// </summary>
		public void SetPencilCaseList(PencilCaseServerData pencilCaseDatas)
		{
			testList = pencilCaseDatas;
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
		/// ���� ���� Ÿ���� ���� �����͸� ��ȯ�Ѵ�
		/// </summary>
		/// <param name="pencilCaseType"></param>
		/// <returns></returns>
		public static PencilCaseData FindPencilCaseData(PencilCaseType pencilCaseType)
		{
			return _havePencilCaseDataList.Find(x => x._pencilCaseType == pencilCaseType);
		}

		/// <summary>
		/// �ΰ��� ���� ����
		/// </summary>
		public static void SetInGamePencilCase(PencilCaseType pencilCaseType)
		{
			_inGamePencilCaseData = FindPencilCaseData(pencilCaseType);
		}

	}
}