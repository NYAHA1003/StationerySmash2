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
		private TextMeshProUGUI _nameText = null;
		[SerializeField]
		private TextMeshProUGUI _descriptionText = null;
		[SerializeField]
		private Image _cardImage;

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

		//��Ųâ 
		[SerializeField]
		private GameObject _skinButtonPrefeb = null;
		[SerializeField]
		private Transform _buttonParent = null;

		//������Ʈ��
		[SerializeField]
		private UserDeckDataComponent _userDeckData; // ���� ������ ������Ʈ
		[SerializeField]
		private DeckSettingComponent _deckSettingComponent; //�� ���� ������Ʈ


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
			_selectCardData = deckCard._cardData;
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
		}

		/// <summary>
		/// �ߵ��� ī���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardExecute(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			//�̸�, �̹���, ���� ����
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;
		}
		/// <summary>
		/// ���� ��ȯ�� ī���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonUnit(CardData cardData)
		{
			_unitStatTexts.SetActive(true);
			_stickerPanel.SetActive(true);

			//�̸�, �̹���, ���� ����
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;

			//���� �ؽ�Ʈ ����
			_hpText.text = cardData.unitData.unit_Hp.ToString();
			_attackText.text = cardData.unitData.damage.ToString();
			_attackSpeedText.text = cardData.unitData.attackSpeed.ToString();
			_moveSpeedText.text = cardData.unitData.moveSpeed.ToString();
			_weightText.text = cardData.unitData.unit_Weight.ToString();
		}
		/// <summary>
		/// ���� ��ȯ���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonTrap(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			//�̸�, �̹���, ���� ����
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;
		}
		/// <summary>
		/// ��ġ���� UI ����
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardInstallation(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			//�̸�, �̹���, ���� ����
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;
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

			//��Ų ��ư�� ����
			for (int i = 0; i < skinList.Count; i++)
			{
				Button skinButton = null;
				if (_buttonParent.GetChild(i) != null)
				{
					skinButton = _buttonParent.GetChild(i).GetComponent<Button>();
				}
				else
				{
					skinButton = Instantiate(_skinButtonPrefeb, _buttonParent).GetComponent<Button>();
				}

				skinButton.onClick.RemoveAllListeners();
				skinButton.GetComponent<CardChangeSkinButton>().SetButtonImages(skinList[i], cardData._cardNamingType);

				//��Ų �Լ����� �־��ش�
				skinButton.onClick.AddListener(() => OnSetSkin(skinList[i], cardData._cardNamingType));
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
				_selectCardData.skinData = getSkinData;
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