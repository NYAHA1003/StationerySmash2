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
	private List<UnitSign> _signListTooLight = new List<UnitSign>();
	private List<UnitSign> _signListLight = new List<UnitSign>();
	private List<UnitSign> _signListMiddle = new List<UnitSign>();
	private List<UnitSign> _signListHeavy = new List<UnitSign>();
	private List<UnitSign> _signListTooHeavy = new List<UnitSign>();

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
			int type = 0;
			_unitSigns[i].SetSigh(_ingameCardDatas.cardDatas[i], out type);
			switch(type)
			{
				case 0:
					_signListTooLight.Add(_unitSigns[i]);
					_unitSigns[i].SetY(40 - _signListTooLight.Count * 60);
					break;
				case 1:
					_signListLight.Add(_unitSigns[i]);
					_unitSigns[i].SetY(40 - _signListLight.Count * 60);
					break;
				case 2:
					_signListMiddle.Add(_unitSigns[i]);
					_unitSigns[i].SetY(40 - _signListMiddle.Count * 60);
					break;
				case 3:
					_signListHeavy.Add(_unitSigns[i]);
					_unitSigns[i].SetY(40 - _signListHeavy.Count * 60);
					break;
				case 4:
					_signListTooHeavy.Add(_unitSigns[i]);
					_unitSigns[i].SetY(40 - _signListTooHeavy.Count * 60);
					break;
			}
		}
	}
}
