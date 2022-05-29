using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Utill.Data
{
	[System.Serializable]
	public class StickerData : IDeepCopy<StickerData>
	{
		//프로퍼티
		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}
		public string Name => _name; //이름
		public string Description => _decription; //설명
		public SkinType SkinType => _skinType; //스킨타입
		public StickerType StickerType => _stickerType; //스티커타입
		public UnitType OnlyUnitType => _onlyUnitType; //제한되는 유닛 타입

		//속성
		private UnitType _onlyUnitType = UnitType.None;
		private StickerType _stickerType = StickerType.None;
		private SkinType _skinType = SkinType.SpriteNone;
		private string _name = "";
		private string _decription = "";
		private int _level = 1;


		/// <summary>
		/// 데이터를 없앤다
		/// </summary>
		public void ReleaseData()
		{
			_stickerType = StickerType.None;
			_level = 0;
			_skinType = SkinType.SpriteNone;
			_name = "";
			_decription = "";
		}

		/// <summary>
		/// 유닛 타입에 따른 스티커 위치 반환
		/// </summary>
		public static Vector2 ReturnStickerPos(UnitType unitType)
		{
			switch (unitType)
			{
				case UnitType.None:
					return Vector2.zero;
			}
			return Vector2.zero;
		}

		/// <summary>
		/// 스티커를 사용할 수 있는지 체크
		/// </summary>
		/// <returns></returns>
		public static bool CheckCanSticker(CardData cardData)
		{
			if (cardData.cardType == CardType.SummonUnit)
			{
				UnitData unitData = UnitDataManagerSO.FindUnitData(cardData.unitType);
				if(unitData == null)
				{
					return false;
				}
				if (unitData._stickerType != StickerType.None)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 스티커 데이터를 복제해서 반환한다
		/// </summary>
		/// <returns></returns>
		public StickerData DeepCopy()
		{
			StickerData stickerData = new StickerData
			{
				_skinType = this._skinType,
				_name = this._name,
				_decription = this._decription,
				_stickerType = this._stickerType,
				_level = this._level,
			};

			return stickerData;
		}
	}

	[System.Serializable]
	public class StickerSaveData
	{
		public int _level;
		public StickerType _stickerType;
	}

	public enum StickerType
	{
		None = 0,
		Rock = 1,
		Paper,
		Scissors,
		Eraser,
		Armor,
		Run,
		Heal,
		LongSee,
		Heavy,
		Invincible,
		PencilNew,
		ScissorPinking,
		RustyRuller,
		PlasticPen,
	}
}
