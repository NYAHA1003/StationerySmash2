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
		private GameObject _cardInfoPanel = null; //패널

		//좌측 카드
		[SerializeField]
		private TextMeshProUGUI _nameText = null;
		[SerializeField]
		private TextMeshProUGUI _descriptionText = null;
		[SerializeField]
		private Image _cardImage;

		//기타
		[SerializeField]
		private Button _equipButton = null; //장착버튼
		[SerializeField]
		private TextMeshProUGUI _equipText = null;
		[SerializeField]
		private CardDescriptionScroll _infoScroll = null; //스크롤 조절창

		//스탯패널(유닛)
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

		//스티커 창
		[SerializeField]
		private GameObject _stickerPanel = null; //스티커 착용창 유닛일 때만 사용
		[SerializeField]
		private StickerInfoPanel _stickerInfoPanel = null;

		//스킨창 
		[SerializeField]
		private GameObject _skinButtonPrefeb = null;
		[SerializeField]
		private Transform _buttonParent = null;

		//컴포넌트들
		[SerializeField]
		private UserDeckDataComponent _userDeckData; // 유저 데이터 컴포넌트
		[SerializeField]
		private DeckSettingComponent _deckSettingComponent; //덱 설정 컴포넌트


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


		//카드별 UI 설정

		/// <summary>
		/// 카드데이터 설정
		/// </summary>
		public void OnSetCardInfoPanel(DeckCard deckCard)
		{
			_cardInfoPanel.SetActive(true);
			_selectCardData = deckCard._cardData;
			SetEquipText();

			//카드 타입에 따라 설명창 설정
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
		/// 발동형 카드의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardExecute(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			//이름, 이미지, 설명 설정
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;
		}
		/// <summary>
		/// 유닛 소환형 카드의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonUnit(CardData cardData)
		{
			_unitStatTexts.SetActive(true);
			_stickerPanel.SetActive(true);

			//이름, 이미지, 설명 설정
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;

			//스탯 텍스트 설정
			_hpText.text = cardData.unitData.unit_Hp.ToString();
			_attackText.text = cardData.unitData.damage.ToString();
			_attackSpeedText.text = cardData.unitData.attackSpeed.ToString();
			_moveSpeedText.text = cardData.unitData.moveSpeed.ToString();
			_weightText.text = cardData.unitData.unit_Weight.ToString();
		}
		/// <summary>
		/// 함정 소환형의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonTrap(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			//이름, 이미지, 설명 설정
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;
		}
		/// <summary>
		/// 설치형의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardInstallation(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			//이름, 이미지, 설명 설정
			_nameText.text = cardData.card_Name;
			_cardImage.sprite = SkinData.GetSkin(cardData.skinData._skinType);
			_descriptionText.text = cardData.card_Description;
		}

		//스킨 함수들

		/// <summary>
		/// 카드가 가진 스킨리스트 생성
		/// </summary>
		public void SetSkinList(CardData cardData)
		{
			_selectCardData = cardData;

			//선택한 유닛의 스킨 리스트 가져오기
			List<SkinData> skinList = SkinData.GetSkinDataList(_selectCardData._cardNamingType);

			//스킨 버튼들 생성
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

				//스킨 함수들을 넣어준다
				skinButton.onClick.AddListener(() => OnSetSkin(skinList[i], cardData._cardNamingType));
			}
		}

		/// <summary>
		/// 스킨을 가지고 있다면 적용
		/// </summary>
		/// <param name="skinData"></param>
		public void OnSetSkin(SkinData skinData, CardNamingType cardNamingType)
		{
			//인벤토리에 해당 스킨을 가지고 오기
			SkinData getSkinData = SkinData.GetSkinDataList(cardNamingType)?.Find(x => x._skinType == skinData._skinType);

			//스킨데이터가 있다면 유닛 데이터의 스킨데이터를 변경
			if (getSkinData != null)
			{
				_selectCardData.skinData = getSkinData;
			}

		}

		//장착 함수들

		/// <summary>
		/// 덱에 카드를 장착하거나 해제한다
		/// </summary>
		public void OnEquipCardInDeck()
		{
			if (_userDeckData.ReturnAlreadyEquipCard(_selectCardData._cardNamingType))
			{
				//해제
				_userDeckData.RemoveCardInDeck(_selectCardData._cardNamingType);
			}
			else
			{
				//장착
				_userDeckData.AddCardInDeck(_selectCardData, _selectCardData.level);
			}
			SetEquipText();

		}

		/// <summary>
		/// 장착 텍스트 설정
		/// </summary>
		public void SetEquipText()
		{
			if (_selectCardData == null)
			{
				return;
			}
			if (_userDeckData.ReturnAlreadyEquipCard(_selectCardData._cardNamingType))
			{
				_equipText.text = "해제";
			}
			else
			{
				_equipText.text = "장착";
			}
		}
	}
}