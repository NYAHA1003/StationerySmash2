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
using UnityEngine.Video;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Main.Card
{
	public class CardInfoPanel : MonoBehaviour
	{
		[SerializeField]
		private GameObject _cardInfoPanel = null; //�г�

		//���� ī��
		[SerializeField]
		private DeckCard _deckCard = null;

		//��Ÿ
		[SerializeField]
		private Button _equipButton = null; //������ư
		private Sequence _equipButtonSequence = null; //������ư �ִϸ��̼� ������
		[SerializeField]
		private TextMeshProUGUI _equipText = null;
		[SerializeField]
		private CardDescriptionScroll _infoScroll = null; //��ũ�� ����â
		[SerializeField]
		private Image _expGaugeBar; //EXP ������ ��
		[SerializeField]
		private GameObject _levelUpArrow; //������ ȭ��ǥ
		[SerializeField]
		private TextMeshProUGUI _levelText; //�����ؽ�Ʈ
		[SerializeField]
		private TextMeshProUGUI _expText; //�����ؽ�Ʈ
		[SerializeField]
		private Button _levelUpButton; //��������ư
		[SerializeField]
		private GameObject _levelUpDontImage; //������ ���� �̹���
		[SerializeField]
		private TextMeshProUGUI _levelUpMoneyText = null; //�������� �ʿ��� �ؽ�Ʈ
		[SerializeField]
		private TextMeshProUGUI _descriptionText = null; //���� �ؽ�Ʈ

		//�����г�(����)
		[SerializeField]
		private GameObject _unitStatTexts = null;
		[SerializeField]
		private TextMeshProUGUI _hpText = null;
		[SerializeField]
		private TextMeshProUGUI _attackText = null;
		[SerializeField]
		private TextMeshProUGUI _attackSpeedText = null;
		[SerializeField]
		private TextMeshProUGUI _moveSpeedText = null;
		[SerializeField]
		private TextMeshProUGUI _weightText = null;
		[SerializeField]
		private TextMeshProUGUI _plusHpText = null;
		[SerializeField]
		private TextMeshProUGUI _plusAttackText = null;
		[SerializeField]
		private TextMeshProUGUI _plusAttackSpeedText = null;
		[SerializeField]
		private TextMeshProUGUI _plusMoveSpeedText = null;
		[SerializeField]
		private TextMeshProUGUI _plusWeightText = null;

		//�̸����� â
		[SerializeField]
		private PreviewVideoControll _videoPlayer = null;

		//��ƼĿ â
		[SerializeField]
		private GameObject _stickerPanel = null; //��ƼĿ ����â ������ ���� ���
		[SerializeField]
		private StickerInfoPanel _stickerInfoPanel = null;
		[SerializeField]
		private GameObject _sktickerButtonPrefeb = null;
		[SerializeField]
		private Transform _stickerButtonParent = null;

		//��Ųâ 
		[SerializeField]
		private GameObject _skinButtonPrefeb = null;
		[SerializeField]
		private Transform _skinButtonParent = null;

		//������Ʈ��
		[SerializeField]
		private UserDeckDataComponent _userDeckData; // ���� ������ ������Ʈ
		[SerializeField]
		private DeckSettingComponent _deckSettingComponent; //�� ���� ������Ʈ
		[SerializeField]
		private WarrningComponent _warrningComponent; //��� ������Ʈ

		//ī�� �����͵�
		[SerializeField]
		private CardDeckSO _haveDeckSO; //������ ī�� ����Ʈ

		private DeckCard _selectDeckCard = null;
		private CardData _selectCardData = null;

		private void Awake()
		{
			EventManager.Instance.StartListening(EventsType.ActiveCardDescription, (x) => OnSetCardInfoPanel((DeckCard)x));
		}

		private void Start()
		{
			_userDeckData ??= FindObjectOfType<UserDeckDataComponent>();
			_deckSettingComponent ??= FindObjectOfType<DeckSettingComponent>();
			_warrningComponent ??= FindObjectOfType<WarrningComponent>();

			SetEquipText();
			_equipButton.onClick.AddListener(() =>
			{
				OnEquipCardInDeck();
				_deckSettingComponent.UpdateHaveAndEquipDeck();
			});

			_levelUpButton.onClick.AddListener(() => OnLevelUp());
		}


		//ī�庰 UI ����

		/// <summary>
		/// ī�嵥���� ����
		/// </summary>
		public void OnSetCardInfoPanel(DeckCard deckCard)
		{
			_cardInfoPanel.SetActive(true);
			_selectDeckCard = deckCard;
			_selectCardData = DeckDataManagerSO.FindHaveCardData(deckCard._cardNamingType);
			SetEquipText();

			//ī�� Ÿ�Կ� ���� ����â ����
			switch (_selectCardData._cardType)
			{
				case CardType.Execute:
					SetCardExecute(_selectCardData);
					break;
				case CardType.SummonUnit:
					SetCardSummonUnit(_selectCardData);
					break;
				case CardType.SummonTrap:
					SetCardSummonTrap(_selectCardData);
					break;
				case CardType.Installation:
					SetCardInstallation(_selectCardData);
					break;
			}
			SetSkinList(_selectCardData);
			SetStickerList(_selectCardData);
			SetViedo(_selectCardData._cardNamingType);
			SetExpBar();
			_deckCard.SetCard(_selectCardData);
		}

		/// <summary>
		/// �ߵ��� ī���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardExecute(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			_infoScroll.SetIcons(3);
		}
		/// <summary>
		/// ���� ��ȯ�� ī���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonUnit(CardData cardData)
		{
			_unitStatTexts.SetActive(true);
			_stickerPanel.SetActive(true);
			_descriptionText.text = $"{cardData._description}";

			//��ƼĿ �г� ����
			_infoScroll.SetIcons(4);

			//���� �ؽ�Ʈ ����
			UnitData unitData = UnitDataManagerSO.FindHaveUnitData(_selectCardData._unitType);
			_hpText.text = unitData._hp.ToString();
			_attackText.text = unitData._damage.ToString();
			_attackSpeedText.text = unitData._attackSpeed.ToString();
			_moveSpeedText.text = unitData._moveSpeed.ToString();

			if (unitData._weight <= 40)
			{
				_weightText.text = "���� ������";
			}
			else if (unitData._weight <= 80)
			{
				_weightText.text = "������";
			}
			else if (unitData._weight <= 120)
			{
				_weightText.text = "����";
			}
			else if (unitData._weight <= 160)
			{
				_weightText.text = "���ſ�";
			}
			else
			{
				_weightText.text = "���� ���ſ�";
			}
		}
		/// <summary>
		/// ���� ��ȯ���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonTrap(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			_infoScroll.SetIcons(3);
		}
		/// <summary>
		/// ��ġ���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardInstallation(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			_infoScroll.SetIcons(3);
		}

		/// <summary>
		/// ������ �Լ�
		/// </summary>
		public void OnLevelUp()
		{
			if (UserSaveManagerSO.UserSaveData._money < GetUpgradeMoney(_selectCardData._level))
			{
				_warrningComponent.SetWarrning("���� �����մϴ�");
			}
			else if (_selectCardData._count >= GetUpgradeCard(_selectCardData._level))
			{
				UnitData unitData = UnitDataManagerSO.FindHaveUnitData(_selectCardData._unitType);
				unitData._hp += unitData._hp * _selectCardData._level / 10;
				unitData._damage += unitData._damage / 10 * _selectCardData._level;
				_selectCardData._count = 1;
				_selectCardData._level++;

				//ī�� Ÿ�Կ� ���� ����â ����
				switch (_selectCardData._cardType)
				{
					case CardType.Execute:
						SetCardExecute(_selectCardData);
						break;
					case CardType.SummonUnit:
						SetCardSummonUnit(_selectCardData);
						break;
					case CardType.SummonTrap:
						SetCardSummonTrap(_selectCardData);
						break;
					case CardType.Installation:
						SetCardInstallation(_selectCardData);
						break;
				}

				SetExpBar();
			}
		}

		/// <summary>
		/// EXP�� ����
		/// </summary>
		private void SetExpBar()
		{
			_levelText.text = $"LV.{_selectCardData._level}";
			_expText.text = $"{_selectCardData._count} / {GetUpgradeCard(_selectCardData._level)}";
			float expPercent = (float)_selectCardData._count / GetUpgradeCard(_selectCardData._level);
			_expGaugeBar.fillAmount = expPercent;
			bool levelUpOn = expPercent >= 1;

			_levelUpArrow.SetActive(levelUpOn);
			_levelUpButton.interactable = levelUpOn;
			_levelUpMoneyText.text = $"{GetUpgradeMoney(_selectCardData._level)}��";
			_levelUpDontImage.SetActive(!levelUpOn);

			UnitData unitData = UnitDataManagerSO.FindHaveUnitData(_selectCardData._unitType);

			_plusHpText.text = $"+{unitData._hp * _selectCardData._level / 10}";
			_plusAttackText.text = $"+{unitData._damage / 10 * _selectCardData._level}";
			_plusHpText.gameObject.SetActive(levelUpOn);
			_plusAttackText.gameObject.SetActive(levelUpOn);
		}

		//��ƼĿ �Լ���

		/// <summary>
		/// ���� ī�忡 ������ �� �ִ� ��ƼĿ ����Ʈ ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetStickerList(CardData cardData)
		{
			//������ ������ ��ƼĿ ����Ʈ ��������
			var commonStickerList = StickerDataManagerSO.FindStickerDataNoneTypeList();
			var onlyUnitStickerList = StickerDataManagerSO.FindStickerDataOnlyUnitTypeList(cardData._unitType);

			int commonCount = commonStickerList?.Count ?? 0;
			int onlyCount = onlyUnitStickerList?.Count ?? 0;

			//��� ��ƼĿ ��ư ����
			for (int i = 0; i < _stickerButtonParent.childCount; i++)
			{
				_stickerButtonParent.GetChild(i).gameObject.SetActive(false);
			}
			//���� ��ƼĿ ��ư�� ����
			for (int i = 0; i < commonCount + onlyCount; i++)
			{
				int j = i;

				StickerData stickerData = null;
				if (j >= commonCount)
				{
					//���� ���� ��ƼĿ ������
					stickerData = onlyUnitStickerList[j - commonCount];
				}
				else
				{
					//���� ��ƼĿ ������
					stickerData = commonStickerList[j];
				}
				Button stickerButton = null;
				if (_stickerButtonParent.childCount > i)
				{
					stickerButton = _stickerButtonParent.GetChild(i).GetComponent<Button>();
				}
				else
				{
					stickerButton = Instantiate(_sktickerButtonPrefeb, _stickerButtonParent).GetComponent<Button>();
				}
				stickerButton.gameObject.SetActive(true);


				stickerButton.onClick.RemoveAllListeners();
				stickerButton.GetComponent<StickerChangeButton>().SetButtonImages(stickerData);

				//��ƼĿ �Լ����� �־��ش�
				stickerButton.onClick.AddListener(() => _stickerInfoPanel.OnSetSkickerPanel(stickerData));
			}
		}

		/// <summary>
		/// �̹� ���� ��ƼĿ�� �����Ǿ����� Ȯ��
		/// </summary>
		/// <returns></returns>
		public bool CheckAlreadyEquipSticker(StickerData stickerData)
		{
			UnitData unitData = UnitDataManagerSO.FindStdUnitData(_selectCardData._unitType);
			if (unitData._stickerType == stickerData._stickerType)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// ��ƼĿ �����͸� ī�� �����Ϳ� ����
		/// </summary>
		/// <param name="stickerData"></param>
		public void SetSticker(StickerData stickerData)
		{
			UnitData unitData = UnitDataManagerSO.FindStdUnitData(_selectCardData._unitType);
			unitData._stickerType = stickerData._stickerType;
			_selectDeckCard.SetCard(_selectCardData);
			_userDeckData.ChangeCardInInGameSaveData(_selectCardData);
		}

		/// <summary>
		/// ��ƼĿ�� �����Ѵ�
		/// </summary>
		public void ReleaseSticker()
		{
			UnitData unitData = UnitDataManagerSO.FindStdUnitData(_selectCardData._unitType);
			unitData._stickerType = StickerType.None;
			_selectDeckCard.SetCard(_selectCardData);
			_userDeckData.ChangeCardInInGameSaveData(_selectCardData);
		}

		//��Ų �Լ���

		/// <summary>
		/// ī�尡 ���� ��Ų����Ʈ ����
		/// </summary>
		public void SetSkinList(CardData cardData)
		{
			_selectCardData = cardData;

			//������ ������ ��Ų ����Ʈ ��������
			List<SkinData> skinList = SkinData.GetSkinDataList(_selectCardData._cardNamingType);

			//��� ��Ų ��ư ����
			for (int i = 0; i < _skinButtonParent.childCount; i++)
			{
				_skinButtonParent.GetChild(i).gameObject.SetActive(false);
			}

			//��Ų ��ư�� ����
			for (int i = 0; i < skinList.Count; i++)
			{
				int j = i;
				SkinData skinData = skinList[j];
				Button skinButton = null;
				if (_skinButtonParent.childCount > i)
				{
					skinButton = _skinButtonParent.GetChild(i).GetComponent<Button>();
				}
				else
				{
					skinButton = Instantiate(_skinButtonPrefeb, _skinButtonParent).GetComponent<Button>();
				}
				skinButton.gameObject.SetActive(true);
				skinButton.onClick.RemoveAllListeners();
				skinButton.GetComponent<CardChangeSkinButton>().SetButtonImages(skinData, cardData._cardNamingType);

				//��Ų �Լ����� �־��ش�
				skinButton.onClick.AddListener(() => OnSetSkin(skinData, cardData._cardNamingType));
			}
		}

		/// <summary>
		/// ��Ų�� ������ �ִٸ� ����
		/// </summary>
		/// <param name="skinData"></param>
		public void OnSetSkin(SkinData skinData, CardNamingType cardNamingType)
		{
			//�κ��丮�� �ش� ��Ų�� ������ ����
			SkinData getSkinData = SkinData.GetSkinDataList(cardNamingType)?.Find(x => x._skinType == skinData._skinType);

			//��Ų�����Ͱ� �ִٸ� ���� �������� ��Ų�����͸� ����
			if (getSkinData != null)
			{
				_selectCardData._skinData = getSkinData;
				_selectDeckCard.SetCard(_selectCardData);
				_deckCard.SetCard(_selectCardData);
			}
		}

		//���� �Լ���

		/// <summary>
		/// ���� ī�带 �����ϰų� �����Ѵ�
		/// </summary>
		public void OnEquipCardInDeck()
		{
			if (_userDeckData.ReturnAlreadyEquipCard(_selectCardData._cardNamingType))
			{
				//����
				_userDeckData.RemoveCardInDeck(_selectCardData._cardNamingType);
			}
			else
			{
				//����
				_userDeckData.AddCardInDeck(_selectCardData, _selectCardData._level);
			}
			SetEquipText();
			EquipButtonAnimation();
			UserSaveManagerSO.PostUserSaveData();
		}

		/// <summary>
		/// ���� �ؽ�Ʈ ����
		/// </summary>
		public void SetEquipText()
		{
			if (_selectCardData == null)
			{
				return;
			}
			if (_userDeckData.ReturnAlreadyEquipCard(_selectCardData._cardNamingType))
			{
				_equipText.text = "����";
			}
			else
			{
				_equipText.text = "����";
			}
		}

		/// <summary>
		/// ���� ��ư �ִϸ��̼�
		/// </summary>
		private void EquipButtonAnimation()
		{
			_equipButton.interactable = false;
			if (_equipButtonSequence == null)
			{
				RectTransform rectTransform = _equipButton.GetComponent<RectTransform>();
				_equipButtonSequence = DOTween.Sequence()
					.SetAutoKill(false)
					.Append(rectTransform.DOScale(new Vector3(0.2f, 0.2f, 0), 0.2f))
					.Append(rectTransform.DOScale(Vector3.one, 0.5f))
					.AppendCallback(() => _equipButton.interactable = true);
			}
			else
			{
				_equipButtonSequence.Restart();
			}
		}

		/// <summary>
		/// ���� ����
		/// </summary>
		private void SetViedo(CardNamingType cardNamingType)
		{
			_videoPlayer.SetVideo(cardNamingType);
		}

		/// <summary>
		/// ��ȭ�� �ʿ��� ī�� ���� ���ϱ�
		/// </summary>
		/// <param name="n"></param>
		/// <param name="first"></param>
		private int GetUpgradeCard(int n)
		{
			int nData = 0;
			int b = 2;

			for (int i = 0; i < n; i++)
			{
				nData = nData + b;
				if (i > 11)
				{
					b = 2000;
				}
				else if (i > 9)
				{
					b *= 3;
				}
				else if (i > 6)

				{
					b *= 2;
				}
				else if (i > 3)
				{
					b += 20;
				}
				else
				{
					b += 4;
				}

			}
			return nData;
		}


		/// <summary>
		/// ��ȭ�� �ʿ��� �� ���ϱ�
		/// </summary>
		/// <param name="n"></param>
		/// <param name="first"></param>
		private int GetUpgradeMoney(int n)
		{
			int nData = 0;
			switch(n)
			{
				case 0:
					nData = 0;
					break;
				case 1:
					nData = 5;
					break;
				case 2:
					nData = 20;
					break;
				case 3:
					nData = 50;
					break;
				case 4:
					nData = 150;
					break;
				case 5:
					nData = 400;
					break;
				case 6:
					nData = 1000;
					break;
				case 7:
					nData = 2000;
					break;
				case 8:
					nData = 4000;
					break;
				case 9:
					nData = 8000;
					break;
				case 10:
					nData = 20000;
					break;
				default:
					nData = 50000;
					break;
			}
			return nData;
		}
	}
}