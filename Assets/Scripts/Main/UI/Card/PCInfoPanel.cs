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

		//�г�
		[SerializeField]
		private GameObject _pcPanel = null;

		//���� �ؽ�Ʈ
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

		//���� �ؽ�Ʈ
		[SerializeField]
		private TextMeshProUGUI _descriptionText = null;

		//���� �г�
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

		//������Ʈ
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
		/// ���� ����â ����
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
		/// ���� ����
		/// </summary>
		public void OnChangePencilCase()
		{
			PencilCaseDataManagerSO.SetInGamePencilCase(_selectPCData._pencilCaseType);
		}

		//�����г�

		/// <summary>
		/// ���� ������ �� �ִ� ���� ����Ʈ ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetBadgeList()
		{
			//��� ���� ��ư ����
			for (int i = 0; i < _badgeButtonParent.childCount; i++)
			{
				_badgeButtonParent.GetChild(i).gameObject.SetActive(false);
			}

			int count = _haveBadgeListSO._badgeLists.Count;
			//���� ���� ��ư�� ����
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

				//���� �Լ����� �־��ش�
				badgeButton.onClick.AddListener(() => _badgeInfoPanel.OnSetBadgePanel(badgeData));
			}
		}


		/// <summary>
		/// ������ �̹� �����Ǿ����� Ȯ���Ѵ�
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
		/// ������ �����Ѵ�
		/// </summary>
		public void AddBadge(BadgeData badgeData)
		{
			if (_selectPCData._badgeDatas.Count == _selectPCData._maxBadgeCount)
			{
				_warrningComponent.SetWarrning("�� �̻� ������ ������ �� �����ϴ�");
				return;
			}
			_selectPCData._badgeDatas.Add(badgeData);
		}



		/// <summary>
		/// ������ �����Ѵ�
		/// </summary>
		public void RemoveBadge(BadgeData badgeData)
		{
			_selectPCData._badgeDatas.Remove(badgeData);
		}

	}

}
