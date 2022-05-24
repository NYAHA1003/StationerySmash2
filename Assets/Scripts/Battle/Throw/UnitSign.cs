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
	public void SetSigh(CardData cardData)
	{
		_unitImage.sprite = SkinData.GetSkin(cardData._skinData._skinType);
		_weight = UnitDataManagerSO.FindUnitData(cardData.unitType).unit_Weight;
		Vector2 rect = _rectTransform.anchoredPosition;
		rect.x = Mathf.Lerp(-800f, 800f, (float)_weight / 200);

		_rectTransform.anchoredPosition = rect;

		gameObject.SetActive(true);
	}
}
