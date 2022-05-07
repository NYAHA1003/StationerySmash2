using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{

	[CreateAssetMenu(fileName = "StickerDataSO", menuName = "Scriptable Object/StickerDataSO")]
	public class StickerDataSO : ScriptableObject
	{
		public List<StickerDataList> _stickerDataLists = new List<StickerDataList>();
	}

	[System.Serializable]
	public class StickerDataList
	{
		public UnitType _onlyUnitType;
		public List<StickerData> _stickerDatas;

		public StickerDataList CopyEmptryList()
		{
			StickerDataList stickerDataList = new StickerDataList
			{
				_onlyUnitType = this._onlyUnitType,
				_stickerDatas = new List<StickerData>(),
			};

			return stickerDataList;
		}

	}
}
