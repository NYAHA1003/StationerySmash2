using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Deck;
using Utill.Data;
using Utill.Tool;
using TMPro;

public class DalgonaToMoney : MonoBehaviour
{
	//인스펙터 참조 변수
	[SerializeField]
	private TextMeshProUGUI _priceText;
	[SerializeField]
	private TextMeshProUGUI _nameText;
	[SerializeField]
	private int _price;
	[SerializeField]
	private int _reward;

	//변수
	private Button _button;
	 

	private void Start()
	{
		_button = GetComponent<Button>();
		Setting();
		_button.onClick.AddListener(() => OnBuy());
	}

	/// <summary>
	/// 버튼 텍스트 설정
	/// </summary>
	private void Setting()
	{
		_priceText.text = $"가격: {_price}";
		_nameText.text = $"{_reward}원";
	}

	/// <summary>
	/// 아이템 구매
	/// </summary>
	private void OnBuy()
	{
		UserSaveManagerSO.AddMoney(_reward);
		UserSaveManagerSO.AddDalgona(-_price);
	}
}
