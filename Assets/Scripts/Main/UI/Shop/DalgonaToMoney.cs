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
	//�ν����� ���� ����
	[SerializeField]
	private TextMeshProUGUI _priceText;
	[SerializeField]
	private TextMeshProUGUI _nameText;
	[SerializeField]
	private int _price;
	[SerializeField]
	private int _reward;

	//����
	private Button _button;
	 

	private void Start()
	{
		_button = GetComponent<Button>();
		Setting();
		_button.onClick.AddListener(() => OnBuy());
	}

	/// <summary>
	/// ��ư �ؽ�Ʈ ����
	/// </summary>
	private void Setting()
	{
		_priceText.text = $"����: {_price}";
		_nameText.text = $"{_reward}��";
	}

	/// <summary>
	/// ������ ����
	/// </summary>
	private void OnBuy()
	{
		UserSaveManagerSO.AddMoney(_reward);
		UserSaveManagerSO.AddDalgona(-_price);
	}
}
