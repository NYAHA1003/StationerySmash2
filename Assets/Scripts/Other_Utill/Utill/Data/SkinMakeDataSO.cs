using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Utill.Data
{

	[CreateAssetMenu(fileName = "SkinMakeDataSO", menuName = "Scriptable Object/SkinMakeDataSO")]
	public class SkinMakeDataSO : ScriptableObject
	{
		public List<SkinMakeDataList> skinMakeDataLists;
	}

	[System.Serializable]
	public class SkinMakeDataList
	{
		public List<SkinMakeData> skinMakeDatas;
	}
}