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

		//뱃지 패널
		[SerializeField]
		private BadgeInfoPanel _badgeInfoPanel = null;
		[SerializeField]
		private GameObject _badgeButtonPrefeb = null;
		[SerializeField]
		private Transform _badgeButtonParent = null;
		[SerializeField]
		private BadgeListSO _haveBadgeListSO = null;

		[SerializeField]
		private UserDeckDataComponent _userDeckDataComponent;

		//컴포넌트
		[SerializeField]
		private WarrningComponent _warrningComponent;

		private void Awake()
		{
			EventManager.Instance.StartListening(EventsType.ChangePencilCase, () => OnChangePencilCase());
			EventManager.Instance.StartListening(EventsType.ActivePencilCaseDescription, (x) => OnSetPCInfoPanel((PencilCaseData)x));
		}

		private void Start()
		{
			_userDeckDataComponent ??= FindObjectOfType<UserDeckDataComponent>();
			_warrningComponent ??= FindObjectOfType<WarrningComponent>();
		}

		/// <summary>
		/// 필통 정보창 열기
		/// </summary>
		/// <param name="pencilCaseData"></param>
		public void OnSetPCInfoPanel(PencilCaseData pencilCaseData)
		{
			_selectPCData = pencilCaseData;

			UnitData unitData = UnitDataManagerSO.FindStdUnitData(pencilCaseData._unitType);
			_nameText.text = $"{System.Enum.GetName(typeof(PencilCaseType), pencilCaseData._pencilCaseType)}";
			_hpText.text = $"{unitData._hp}";
			_maxBadgeText.text = $"{pencilCaseData._maxBadgeCount}";
			_maxCardText.text = $"{pencilCaseData._maxCard}";
			_throwGaugeText.text = $"{pencilCaseData._throwGaugeSpeed}";
			_descriptionText.text = $"{pencilCaseData._description}";

			EventManager.Instance.TriggerEvent(EventsType.ActiveButtonComponent, ButtonType.pencilCaseDescription);
			SetBadgeList();
		}

		/// <summary>
		/// 필통 변경
		/// </summary>
		public void OnChangePencilCase()
		{
			PencilCaseDataManagerSO.SetInGamePencilCase(_selectPCData._pencilCaseType);
		}

		//뱃지패널

		/// <summary>
		/// 필통 장착할 수 있는 뱃지 리스트 생성
		/// </summary>
		/// <param name="cardData"></param>
		public void SetBadgeList()
		{
			//모든 뱃지 버튼 끄기
			for (int i = 0; i < _badgeButtonParent.childCount; i++)
			{
				_badgeButtonParent.GetChild(i).gameObject.SetActive(false);
			}

			int count = _haveBadgeListSO._badgeLists.Count;
			//공용 뱃지 버튼들 생성
			for (int i = 0; i < count; i++)
			{
				int j = i;

				BadgeData badgeData = _haveBadgeListSO._badgeLists[i];
				Button badgeButton = null;
				if (_badgeButtonParent.childCount > i)
				{
					badgeButton = _badgeButtonParent.GetChild(i).GetComponent<Button>();
				}
				else
				{
					badgeButton = Instantiate(_badgeButtonPrefeb, _badgeButtonParent).GetComponent<Button>();
				}
				badgeButton.gameObject.SetActive(true);

				badgeButton.onClick.RemoveAllListeners();
				badgeButton.GetComponent<BadgeSetButton>().SetButtonImages(badgeData);

				//뱃지 함수들을 넣어준다
				badgeButton.onClick.AddListener(() => _badgeInfoPanel.OnSetBadgePanel(badgeData));
			}
		}


		/// <summary>
		/// 뱃지가 이미 장착되었는지 확인한다
		/// </summary>
		public bool CheckAlreadyEquipBadge(BadgeData badgeData)
		{
			if(_selectPCData._badgeDatas.Find(x => x._badgeType == badgeData._badgeType) != null)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 뱃지를 장착한다
		/// </summary>
		public void AddBadge(BadgeData badgeData)
		{
			if (_selectPCData._badgeDatas.Count == _selectPCData._maxBadgeCount)
			{
				_warrningComponent.SetWarrning("더 이상 뱃지를 장착할 수 없습니다");
				return;
			}
			_selectPCData._badgeDatas.Add(badgeData);
		}



		/// <summary>
		/// 뱃지를 제거한다
		/// </summary>
		public void RemoveBadge(BadgeData badgeData)
		{
			_selectPCData._badgeDatas.Remove(badgeData);
		}

	}

}
