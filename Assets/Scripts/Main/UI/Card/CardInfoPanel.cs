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
		private GameObject _cardInfoPanel = null; //패널

		//좌측 카드
		[SerializeField]
		private DeckCard _deckCard = null;

		//기타
		[SerializeField]
		private Button _equipButton = null; //장착버튼
		private Sequence _equipButtonSequence = null; //장착버튼 애니메이션 시퀀스
		[SerializeField]
		private TextMeshProUGUI _equipText = null;
		[SerializeField]
		private CardDescriptionScroll _infoScroll = null; //스크롤 조절창
		[SerializeField]
		private Image _expGaugeBar; //EXP 게이지 바
		[SerializeField]
		private GameObject _levelUpArrow; //레벨업 화살표
		[SerializeField]
		private TextMeshProUGUI _levelText; //레벨텍스트
		[SerializeField]
		private TextMeshProUGUI _expText; //레벨텍스트
		[SerializeField]
		private Button _levelUpButton; //레벨업버튼
		[SerializeField]
		private GameObject _levelUpDontImage; //레벨업 막는 이미지
		[SerializeField]
		private TextMeshProUGUI _levelUpMoneyText = null; //레벨업에 필요한 텍스트
		[SerializeField]
		private TextMeshProUGUI _descriptionText = null; //설명 텍스트

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

		//미리보기 창
		[SerializeField]
		private PreviewVideoControll _videoPlayer = null;

		//스티커 창
		[SerializeField]
		private GameObject _stickerPanel = null; //스티커 착용창 유닛일 때만 사용
		[SerializeField]
		private StickerInfoPanel _stickerInfoPanel = null;
		[SerializeField]
		private GameObject _sktickerButtonPrefeb = null;
		[SerializeField]
		private Transform _stickerButtonParent = null;

		//스킨창 
		[SerializeField]
		private GameObject _skinButtonPrefeb = null;
		[SerializeField]
		private Transform _skinButtonParent = null;

		//컴포넌트들
		[SerializeField]
		private UserDeckDataComponent _userDeckData; // 유저 데이터 컴포넌트
		[SerializeField]
		private DeckSettingComponent _deckSettingComponent; //덱 설정 컴포넌트
		[SerializeField]
		private WarrningComponent _warrningComponent; //경고 컴포넌트

		//카드 데이터들
		[SerializeField]
		private CardDeckSO _haveDeckSO; //보유한 카드 리스트

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


		//카드별 UI 설정

		/// <summary>
		/// 카드데이터 설정
		/// </summary>
		public void OnSetCardInfoPanel(DeckCard deckCard)
		{
			_cardInfoPanel.SetActive(true);
			_selectDeckCard = deckCard;
			_selectCardData = DeckDataManagerSO.FindHaveCardData(deckCard._cardNamingType);
			SetEquipText();

			//카드 타입에 따라 설명창 설정
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
		/// 발동형 카드의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardExecute(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			_infoScroll.SetIcons(3);
		}
		/// <summary>
		/// 유닛 소환형 카드의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonUnit(CardData cardData)
		{
			_unitStatTexts.SetActive(true);
			_stickerPanel.SetActive(true);
			_descriptionText.text = $"{cardData._description}";

			//스티커 패널 설정
			_infoScroll.SetIcons(4);

			//스탯 텍스트 설정
			UnitData unitData = UnitDataManagerSO.FindHaveUnitData(_selectCardData._unitType);
			_hpText.text = unitData._hp.ToString();
			_attackText.text = unitData._damage.ToString();
			_attackSpeedText.text = unitData._attackSpeed.ToString();
			_moveSpeedText.text = unitData._moveSpeed.ToString();

			if (unitData._weight <= 40)
			{
				_weightText.text = "아주 가벼움";
			}
			else if (unitData._weight <= 80)
			{
				_weightText.text = "가벼움";
			}
			else if (unitData._weight <= 120)
			{
				_weightText.text = "보통";
			}
			else if (unitData._weight <= 160)
			{
				_weightText.text = "무거움";
			}
			else
			{
				_weightText.text = "아주 무거움";
			}
		}
		/// <summary>
		/// 함정 소환형의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardSummonTrap(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			_infoScroll.SetIcons(3);
		}
		/// <summary>
		/// 설치형의 UI 설정
		/// </summary>
		/// <param name="cardData"></param>
		public void SetCardInstallation(CardData cardData)
		{
			_unitStatTexts.SetActive(false);
			_stickerPanel.SetActive(false);

			_infoScroll.SetIcons(3);
		}

		/// <summary>
		/// 레벨업 함수
		/// </summary>
		public void OnLevelUp()
		{
			if (UserSaveManagerSO.UserSaveData._money < GetUpgradeMoney(_selectCardData._level))
			{
				_warrningComponent.SetWarrning("돈이 부족합니다");
			}
			else if (_selectCardData._count >= GetUpgradeCard(_selectCardData._level))
			{
				UnitData unitData = UnitDataManagerSO.FindHaveUnitData(_selectCardData._unitType);
				unitData._hp += unitData._hp * _selectCardData._level / 10;
				unitData._damage += unitData._damage / 10 * _selectCardData._level;
				_selectCardData._count = 1;
				_selectCardData._level++;

				//카드 타입에 따라 설명창 설정
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
		/// EXP바 설정
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
			_levelUpMoneyText.text = $"{GetUpgradeMoney(_selectCardData._level)}원";
			_levelUpDontImage.SetActive(!levelUpOn);

			UnitData unitData = UnitDataManagerSO.FindHaveUnitData(_selectCardData._unitType);

			_plusHpText.text = $"+{unitData._hp * _selectCardData._level / 10}";
			_plusAttackText.text = $"+{unitData._damage / 10 * _selectCardData._level}";
			_plusHpText.gameObject.SetActive(levelUpOn);
			_plusAttackText.gameObject.SetActive(levelUpOn);
		}

		//스티커 함수들

		/// <summary>
		/// 유닛 카드에 장착할 수 있는 스티커 리스트 생성
		/// </summary>
		/// <param name="cardData"></param>
		public void SetStickerList(CardData cardData)
		{
			//선택한 유닛의 스티커 리스트 가져오기
			var commonStickerList = StickerDataManagerSO.FindStickerDataNoneTypeList();
			var onlyUnitStickerList = StickerDataManagerSO.FindStickerDataOnlyUnitTypeList(cardData._unitType);

			int commonCount = commonStickerList?.Count ?? 0;
			int onlyCount = onlyUnitStickerList?.Count ?? 0;

			//모든 스티커 버튼 끄기
			for (int i = 0; i < _stickerButtonParent.childCount; i++)
			{
				_stickerButtonParent.GetChild(i).gameObject.SetActive(false);
			}
			//공용 스티커 버튼들 생성
			for (int i = 0; i < commonCount + onlyCount; i++)
			{
				int j = i;

				StickerData stickerData = null;
				if (j >= commonCount)
				{
					//전용 유닛 스티커 데이터
					stickerData = onlyUnitStickerList[j - commonCount];
				}
				else
				{
					//공용 스티커 데이터
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

				//스티커 함수들을 넣어준다
				stickerButton.onClick.AddListener(() => _stickerInfoPanel.OnSetSkickerPanel(stickerData));
			}
		}

		/// <summary>
		/// 이미 같은 스티커가 장착되었는지 확인
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
		/// 스티커 데이터를 카드 데이터에 적용
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
		/// 스티커를 제거한다
		/// </summary>
		public void ReleaseSticker()
		{
			UnitData unitData = UnitDataManagerSO.FindStdUnitData(_selectCardData._unitType);
			unitData._stickerType = StickerType.None;
			_selectDeckCard.SetCard(_selectCardData);
			_userDeckData.ChangeCardInInGameSaveData(_selectCardData);
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

			//모든 스킨 버튼 끄기
			for (int i = 0; i < _skinButtonParent.childCount; i++)
			{
				_skinButtonParent.GetChild(i).gameObject.SetActive(false);
			}

			//스킨 버튼들 생성
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

				//스킨 함수들을 넣어준다
				skinButton.onClick.AddListener(() => OnSetSkin(skinData, cardData._cardNamingType));
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
				_selectCardData._skinData = getSkinData;
				_selectDeckCard.SetCard(_selectCardData);
				_deckCard.SetCard(_selectCardData);
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
				_userDeckData.AddCardInDeck(_selectCardData, _selectCardData._level);
			}
			SetEquipText();
			EquipButtonAnimation();
			UserSaveManagerSO.PostUserSaveData();
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

		/// <summary>
		/// 장착 버튼 애니메이션
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
		/// 비디오 설정
		/// </summary>
		private void SetViedo(CardNamingType cardNamingType)
		{
			_videoPlayer.SetVideo(cardNamingType);
		}

		/// <summary>
		/// 강화에 필요한 카드 갯수 구하기
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
		/// 강화에 필요한 돈 구하기
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