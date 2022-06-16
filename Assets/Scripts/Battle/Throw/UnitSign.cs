using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

public class UnitSign : MonoBehaviour
{
	[SerializeField]
	private Image _unitImage;
	[SerializeField]
	private RectTransform _rectTransform;

	private int _weight;
	
	/// <summary>
	/// 유닛 사인 설정
	/// </summary>
	/// <param name="cardData"></param>
	public void SetSigh(CardData cardData, out int type)
	{
		_unitImage.sprite = SkinData.GetSkin(cardData._skinData._skinType);
		_weight = UnitDataManagerSO.FindStdUnitData(cardData._unitType)._weight;
		Vector2 rect = _rectTransform.anchoredPosition;

		if(_weight <= 40)
		{
			rect.x = -190;
			type = 0;
		}
		else if(_weight <= 80)
		{
			rect.x = -105;
			type = 1;
		}
		else if (_weight <= 120)
		{
			rect.x = 0;
			type = 2;
		}
		else if (_weight <= 160)
		{
			rect.x = 105;
			type = 3;
		}
		else
		{
			rect.x = 190;
			type = 4;
		}

		_rectTransform.anchoredPosition = rect;

		gameObject.SetActive(true);
	}

	/// <summary>
	/// Y 위치 설정
	/// </summary>
	/// <param name="value"></param>
	public void SetY(int value)
	{
		Vector2 rect = _rectTransform.anchoredPosition;
		rect.y = value;
		_rectTransform.anchoredPosition = rect;
	}
}
