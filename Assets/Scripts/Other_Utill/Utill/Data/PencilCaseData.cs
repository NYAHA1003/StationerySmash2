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
		public string _name;
		public string _description;
		public PencilCaseType _pencilCaseType;
		public AbstractPencilCaseAbility _pencilState;
		public SkinType _skinType;
		public UnitType _unitType = UnitType.PencilCase;
		public List<BadgeData> _badgeDatas;

		/// <summary>
		/// 필통 데이터를 세이브데이터 기준으로 변환한다
		/// </summary>
		/// <returns></returns>
		public PencilCaseData DeepCopyNoneBadge()
		{
			PencilCaseData pencilCaseData = new PencilCaseData
			{
				_maxCard = _maxCard,
				_maxBadgeCount = _maxBadgeCount,
				_costSpeed = _costSpeed,
				_throwGaugeSpeed = _throwGaugeSpeed,
				_name = _name,
				_description = _description,
				_pencilCaseType = _pencilCaseType,
				_pencilState = _pencilState,
				_skinType = _skinType,
				_unitType = _unitType,
				_badgeDatas = new List<BadgeData>(),
			};
			return pencilCaseData;
		}

		/// <summary>
		/// 필통의 카드 데이터를 반환한다
		/// </summary>
		/// <returns></returns>
		public CardData ReturnCardData()
		{
			var cardData = new CardData
			{
				_cost = 0,
				_cardType = CardType.SummonUnit,
				_description = "",
				_name = "",
				_cardNamingType = CardNamingType.PencilCase,
				_skinData = new SkinData
				{
					_skinType = this._skinType,
					_effectType = EffectType.Attack
				},
				_level = 1,
				_starategyType = StrategyType.None,
				_unitType = UnitType.PencilCase,
			};

			return cardData;
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

			for (int i = 0; i < pcData._badgeDatas.Count; i++)
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
