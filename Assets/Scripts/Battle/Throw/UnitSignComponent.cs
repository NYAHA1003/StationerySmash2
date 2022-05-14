using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Main.Deck;

[System.Serializable]
public class UnitSignComponent : MonoBehaviour
{
	[SerializeField]
	private List<UnitSign> _unitSigns;
	[SerializeField]
	private CardDeckSO _ingameCardDatas;


	public void SetInitialization()
	{
		SetUnitSigns();
	}

	/// <summary>
	/// 유닛사인들 설정
	/// </summary>
	private void SetUnitSigns()
	{
		for(int i = 0; i < 10; i++)
		{
			_unitSigns[i].gameObject.SetActive(false);
		}

		int count = _ingameCardDatas.cardDatas.Count;
		for(int i = 0; i < count; i++)
		{
			_unitSigns[i].SetSigh(_ingameCardDatas.cardDatas[i]);
		}
	}
}
