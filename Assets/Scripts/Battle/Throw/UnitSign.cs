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
	private int _weight;

	/// <summary>
	/// 유닛 사인 설정
	/// </summary>
	/// <param name="cardData"></param>
	public void SetSigh(CardData cardData)
	{
		_unitImage.sprite = SkinData.GetSkin(cardData._skinData._skinType);
		_weight = cardData.unitData.unit_Weight;
		gameObject.SetActive(true);
	}
}
