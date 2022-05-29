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
		//�Ӽ�
		public UnitType _onlyUnitType = UnitType.None;
		public StickerType _stickerType = StickerType.None;
		public SkinType _skinType = SkinType.SpriteNone;
		public string _name = "";
		public string _decription = "";
		public int _level = 1;


		/// <summary>
		/// �����͸� ���ش�
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
		/// ���� Ÿ�Կ� ���� ��ƼĿ ��ġ ��ȯ
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
		/// ��ƼĿ�� ����� �� �ִ��� üũ
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
		/// ��ƼĿ �����͸� �����ؼ� ��ȯ�Ѵ�
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
