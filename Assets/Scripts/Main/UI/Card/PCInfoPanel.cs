using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;
using Main.Deck;
using Main.Card;
using Main.Event;
using Main.Skin;

namespace Main.Card
{
	public class PCInfoPanel : MonoBehaviour
	{
		private PencilCaseData _selectPCData = null;

		//패널
		[SerializeField]
		private GameObject _pcPanel = null;

		//스탯 텍스트
		[SerializeField]
		private TextMeshProUGUI _nameText = null;
		[SerializeField]
		private TextMeshProUGUI _hpText = null;
		[SerializeField]
		private TextMeshProUGUI _maxCardText = null;
		[SerializeField]
		private TextMeshProUGUI _throwGaugeText = null;
		[SerializeField]
		private TextMeshProUGUI _maxBadgeText = null;

		//설명 텍스트
		[SerializeField]
		private TextMeshProUGUI _descriptionText = null;

		[SerializeField]
		private UserDeckDataComponent _userDeckDataComponent;

		private void Awake()
		{
			EventManager.StartListening(EventsType.ChangePencilCase, () => OnChangePencilCase());
			EventManager.StartListening(EventsType.ActivePencilCaseDescription, (x) => OnSetPCInfoPanel((PencilCaseData)x));
		}

		private void Start()
		{
			_userDeckDataComponent ??= FindObjectOfType<UserDeckDataComponent>();
		}

		/// <summary>
		/// 필통 정보창 열기
		/// </summary>
		/// <param name="pencilCaseData"></param>
		public void OnSetPCInfoPanel(PencilCaseData pencilCaseData)
		{
			_selectPCData = pencilCaseData;

			_nameText.text = $"{System.Enum.GetName(typeof(PencilCaseType), pencilCaseData._pencilCaseType)}";
			_hpText.text = $"{pencilCaseData._pencilCaseData.unitData.unit_Hp}";
			_maxBadgeText.text = $"{pencilCaseData._maxBadgeCount}";
			_maxCardText.text = $"{pencilCaseData._maxCard}";
			_throwGaugeText.text = $"{pencilCaseData._throwGaugeSpeed}";
			_descriptionText.text = $"{pencilCaseData._description}";

			_pcPanel.SetActive(true);
		}


		/// <summary>
		/// 필통 변경
		/// </summary>
		public void OnChangePencilCase()
		{
			_userDeckDataComponent.SetInGamePencilCase(_selectPCData);
		}
	}

}
