using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle.PCAbility;

namespace Utill.Data
{
	[System.Serializable]
	public class PencilCaseData
	{
		public int _maxCard;
		public int _maxBadgeCount;
		public float _costSpeed;
		public float _throwGaugeSpeed;
		public string _description;
		public PencilCaseType _pencilCaseType;
		public AbstractPencilCaseAbility _pencilState;
		public CardData _pencilCaseData;
		public List<BadgeData> _badgeDatas;

		/// <summary>
		/// 필통 데이터를 세이브데이터 기준으로 변환한다
		/// </summary>
		/// <returns></returns>
		public PencilCaseData DeepCopyNoneBadge()
		{
			PencilCaseData pencilCaseData = new PencilCaseData
			{
			_maxCard = this._maxCard,
			_maxBadgeCount = _maxBadgeCount,
			_costSpeed = _costSpeed,
			_throwGaugeSpeed = _throwGaugeSpeed,
			_description = _description,
			_pencilCaseType = _pencilCaseType,
			_pencilState = _pencilState,
			_pencilCaseData = _pencilCaseData,
			_badgeDatas = new List<BadgeData>(),
		};
			return pencilCaseData;
		}
	}

	[System.Serializable]
	public class PencilCaseSaveData
	{
		public PencilCaseType _pencilCaseType;
		public List<BadgeSaveData> _badgeDatas;

		public static PencilCaseSaveData DeepCopyFromPCData(PencilCaseData pcData)
		{
			PencilCaseSaveData pencilCaseSaveData = new PencilCaseSaveData
			{
				_pencilCaseType = pcData._pencilCaseType,
			};

			for(int i = 0; i < pcData._badgeDatas.Count; i++)
			{
				BadgeSaveData badgeSaveData = new BadgeSaveData
				{
					_level = pcData._badgeDatas[i]._level,
					_BadgeType = pcData._badgeDatas[i]._badgeType,
				};
				pencilCaseSaveData._badgeDatas.Add(badgeSaveData);
			}

			return pencilCaseSaveData;
		}
	}
}
