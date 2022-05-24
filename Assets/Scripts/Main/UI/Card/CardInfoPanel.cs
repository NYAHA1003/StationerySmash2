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
		[SerializeField]
		private TextMeshProUGUI _equipText = null;
		[SerializeField]
		private CardDescriptionScroll _infoScroll = null; //��ũ�� ����â

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

		//ī�� �����͵�
		[SerializeField]
		private CardDeckSO _haveDeckSO; //������ ī�� ����Ʈ

		private DeckCard _selectDeckCard = null;
		private CardData _selectCardData = null;

		private void Awake()
		{
			EventManager.StartListening(EventsType.ActiveCardDescription, (x) => OnSetCardInfoPanel((DeckCard)x));
		}

		private void Start()
		{
			SetEquipText();
			_equipButton.onClick.AddListener(() =>
			{
				OnEquipCardInDeck();
				_deckSettingComponent.UpdateHaveAndEquipDeck();
			});
		}


		//ī�庰 UI ����

		/// <summary>
		/// ī�嵥���� ����
		/// </summary>
		public void OnSetCardInfoPanel(DeckCard deckCard)
		{
			_cardInfoPanel.SetActive(true);
			_selectDeckCard = deckCard;
			_selectCardData = _haveDeckSO.cardDatas.Find(x => x._cardNamingType == deckCard._cardNamingType);
			SetEquipText();

			//ī�� Ÿ�Կ� ���� ����â ����
			switch (_selectCardData.cardType)
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

			//��ƼĿ �г� ����
			_infoScroll.SetIcons(4);

			//���� �ؽ�Ʈ ����
			UnitData unitData = UnitDataManagerSO.FindUnitData(cardData.unitType);
			_hpText.text = unitData.unit_Hp.ToString();
			_attackText.text = unitData.damage.ToString();
			_attackSpeedText.text = unitData.attackSpeed.ToString();
			_moveSpeedText.text = unitData.moveSpeed.ToString();
			_weightText.text = unitData.unit_Weight.ToString();
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

		//��ƼĿ �Լ���

		/// <summary>
		/// ���� ī�忡 ������ �� �ִ� ��ƼĿ ����Ʈ ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetStickerList(CardData cardData)
		{
			//������ ������ ��ƼĿ ����Ʈ ��������
			var commonStickerList = StickerDataManagerSO.FindStickerDataNoneTypeList();
			var onlyUnitStickerList = StickerDataManagerSO.FindStickerDataOnlyUnitTypeList(cardData.unitType);

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
				if(j >= commonCount)
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
			UnitData unitData = UnitDataManagerSO.FindUnitData(_selectCardData.unitType);
			if(unitData.stickerType == stickerData.StickerType)
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
			UnitData unitData = UnitDataManagerSO.FindUnitData(_selectCardData.unitType);
			unitData.stickerType = stickerData.StickerType;
			_selectDeckCard.SetCard(_selectCardData);
			_userDeckData.ChangeCardInInGameSaveData(_selectCardData);
		}

		/// <summary>
		/// ��ƼĿ�� �����Ѵ�
		/// </summary>
		public void ReleaseSticker()
		{
			UnitData unitData = UnitDataManagerSO.FindUnitData(_selectCardData.unitType);
			unitData.stickerType = StickerType.None;
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
			for(int i = 0; i < _skinButtonParent.childCount; i++)
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
				_userDeckData.AddCardInDeck(_selectCardData, _selectCardData.level);
			}
			SetEquipText();

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
	}
}