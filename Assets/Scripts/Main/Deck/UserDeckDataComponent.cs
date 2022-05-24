using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Main.Deck;
using Utill.Data;

namespace Main.Deck
{
	public class UserDeckDataComponent : MonoBehaviour, IUserData
	{
		//카드 관련
		public CardDeckSO _standardcardDeckSO; //기준 카드 데이터 
		public CardDeckSO _haveDeckListSO; //보유 카드 데이터
		public CardDeckSO _inGameDeckListSO; //장착 카드 데이터

		//필통 관련
		public PencilCaseDataListSO _standardPCDataSO; //기준 필통 데이터들
		public PencilCaseDataListSO _havePCDataSO; //보유 필통 데이터들
		public PencilCaseDataSO _inGamePCDataSO; //장착 필통 데이터

		//스티커 관련
		public StickerDataSO _standardStickerDataSO; //기준 스티커 데이터들
		public StickerDataSO _haveStickerDataSO; //보유 스티커 데이터들

		//뱃지 관련
		public BadgeListSO _standardBadgeDataSO; //기준 뱃지 데이터들
		public BadgeListSO _haveBadgeDataSO; //보유 뱃지 데이터들

		//스킨 관련
		public SkinListSO _skinListSO; //스킨 데이터들

		//프리셋
		[SerializeField]
		private PresetSaveDataSO _presetDataSO1 = null;
		[SerializeField]
		private PresetSaveDataSO _presetDataSO2 = null;
		[SerializeField]
		private PresetSaveDataSO _presetDataSO3 = null;

		[SerializeField]
		private WarrningComponent _warrningComponent; //경고 컴포넌트

		private UserSaveData _userSaveData; //유저 데이터

		private void Awake()
		{
			SaveManager._instance.SaveData.AddObserver(this);
			_userSaveData ??= SaveManager._instance.SaveData.userSaveData;

			SetCardData();
			SetPencilCaseData();
			ChangePreset(_userSaveData._setPrestIndex);
		}

		public void Notify(ref UserSaveData userSaveData)
		{
			SetCardData();
			SetPencilCaseData();
		}

		/// <summary>
		/// 카드덱 데이터를 세팅해줍니다 
		/// </summary>
		public void SetCardData()
		{
			//카드 데이터 초기화
			SetHaveDeckCardList();
			SetIngameCardList();

			//스티커 데이터 초기화
			SetStickerDataList();

		}


		/// <summary>
		/// 필통 데이터 설정
		/// </summary>
		public void SetPencilCaseData()
		{
			SetPencilCaseList();
			SetInGamePencilCase(_havePCDataSO._pencilCaseDataList.Find(x => x._pencilCaseType == _userSaveData._currentPencilCaseType));

			//뱃지 데이터 초기화
			SetBadgeDataList();
		}


		/// <summary>
		/// 프리셋 변경
		/// </summary>
		/// <param name="index"></param>
		public void ChangePreset(int index)
		{
			PresetSaveDataSO presetSaveDataSO = null;
			switch (index)
			{
				case 0:
					presetSaveDataSO = _presetDataSO1;
					break;
				case 1:
					presetSaveDataSO = _presetDataSO2;
					break;
				case 2:
					presetSaveDataSO = _presetDataSO3;
					break;
			}

			_userSaveData._ingameSaveDatas = presetSaveDataSO._ingameSaveDatas;
			_userSaveData._currentPencilCaseType = presetSaveDataSO._pencilCaseData._pencilCaseType;

			PencilCaseData pencilCaseData = _havePCDataSO._pencilCaseDataList.Find(x => x._pencilCaseType == presetSaveDataSO._pencilCaseData._pencilCaseType);
			PencilCaseData changePCData = null;

			changePCData = pencilCaseData.DeepCopyNoneBadge();
			for (int i = 0; i < presetSaveDataSO._pencilCaseData._badgeDatas.Count; i++)
			{
				changePCData._badgeDatas.Add(_haveBadgeDataSO._badgeLists.Find(x => x._badgeType == presetSaveDataSO._pencilCaseData._badgeDatas[i]._BadgeType));
			}
			SetInGamePencilCase(changePCData);


			_userSaveData._setPrestIndex = index;
		}

		/// <summary>
		/// 보유 카드 데이터 설정
		/// </summary>
		public void SetHaveDeckCardList()
		{
			_haveDeckListSO.cardDatas.Clear();
			int count = _userSaveData._haveCardSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = _userSaveData._haveCardSaveDatas[i];
				//같은 카드 타입 찾기
				CardData cardDataobj = _standardcardDeckSO.cardDatas.Find(x => x._cardNamingType == saveDataobj._cardNamingType);

				if (cardDataobj != null)
				{
					//세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
					SkinData skinData = _skinListSO._cardNamingSkins.Find(x => x._cardNamingType == saveDataobj._cardNamingType)._skinDatas.Find(x => x._skinType == saveDataobj._skinType);
					CardData cardData = cardDataobj.DeepCopy();
					cardData.level = saveDataobj._level;
					cardData._skinData = skinData;
					_haveDeckListSO.cardDatas.Add(cardData);
				}
			}
		}

		/// <summary>
		/// 가나다 순으로 정렬한다
		/// </summary>
		public void HaveDeckSortABC()
		{
			var list = _haveDeckListSO.cardDatas.OrderBy(x => x.card_Name);
			_haveDeckListSO.cardDatas = list.ToList<CardData>();
		}

		/// <summary>
		/// 코스트 순으로 정렬한다
		/// </summary>
		public void HaveDeckSortCost()
		{
			var list = _haveDeckListSO.cardDatas.OrderBy(x => x.card_Cost);
			_haveDeckListSO.cardDatas = list.ToList<CardData>();
		}

		/// <summary>
		/// 보유 필통 설정
		/// </summary>
		public void SetPencilCaseList()
		{
			_havePCDataSO._pencilCaseDataList.Clear();
			int count = _userSaveData._havePencilCaseList.Count;
			for (int i = 0; i < count; i++)
			{
				PencilCaseType pcType = _userSaveData._havePencilCaseList[i];

				PencilCaseData pcData = _standardPCDataSO._pencilCaseDataList.Find(x => x._pencilCaseType == pcType);

				if (pcData != null)
				{
					_havePCDataSO._pencilCaseDataList.Add(pcData);
				}
			}
		}
		/// <summary>
		/// 인게임 필통 설정
		/// </summary>
		public void SetInGamePencilCase(PencilCaseData pencilCaseData)
		{
			_inGamePCDataSO._pencilCaseData = pencilCaseData;
		}

		/// <summary>
		/// 인게임 카드 덱 설정
		/// </summary>
		public void SetIngameCardList()
		{
			_inGameDeckListSO.cardDatas.Clear();
			int count = _userSaveData._ingameSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = _userSaveData._ingameSaveDatas[i];
				//세가지 타입이 세이브데이터와 모두 같은 기준 데이터 찾기
				CardData cardDataobj = _haveDeckListSO.cardDatas.Find(x => x._cardNamingType == saveDataobj._cardNamingType);

				if (cardDataobj != null)
				{
					//세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
					_inGameDeckListSO.cardDatas.Add(cardDataobj);
				}
			}
		}

		/// <summary>
		/// 보유 스티커 데이터를 설정한다
		/// </summary>
		public void SetStickerDataList()
		{
			_haveStickerDataSO.GetStickerDataList().Clear();

			int count = _userSaveData._haveStickerList.Count;
			int categoryCount = _standardStickerDataSO.GetStickerDataList().Count;

			//빈 스티커 리스트 생성
			for (int i = 0; i < categoryCount; i++)
			{
				//스티커 리스트
				var _stickerList = _standardStickerDataSO.GetStickerDataList()[i];
				//빈 스티커 리스트 생성
				_haveStickerDataSO.GetStickerDataList().Add(_stickerList.CopyEmptryList());
			}

			//가지고 있는 스티커 알맞는 스티커 데이터 찾기
			for (int i = 0; i < categoryCount; i++)
			{
				//스티커 리스트
				var _stickerList = _standardStickerDataSO.GetStickerDataList()[i];

				for (int j = 0; j < _userSaveData._haveStickerList.Count; j++)
				{
					StickerSaveData stickerSaveData = _userSaveData._haveStickerList[j];
					StickerData addStickerData = _stickerList._stickerDatas.Find(x => x.StickerType == stickerSaveData._stickerType);
					if(addStickerData != null)
					{
						StickerData stickerData = addStickerData.DeepCopy();
						stickerData.Level = stickerSaveData._level;
						_haveStickerDataSO.GetStickerDataList()[i]._stickerDatas.Add(stickerData);
					}
				}
			}
		}

		/// <summary>
		/// 보유 뱃지 데이터를 설정한다
		/// </summary>
		public void SetBadgeDataList()
		{
			_haveBadgeDataSO._badgeLists.Clear();

			int count = _userSaveData._haveBadgeSaveDatas.Count;

			for (int i = 0; i < count; i++)
			{
				BadgeSaveData badgeSaveData = _userSaveData._haveBadgeSaveDatas[i];
				_haveBadgeDataSO._badgeLists.Add(_standardBadgeDataSO._badgeLists.Find(x => x._badgeType == badgeSaveData._BadgeType).DeepCopyBadgeData(badgeSaveData));
			}
		}

		/// <summary>
		/// 덱에 카드를 추가한다
		/// </summary>
		public void AddCardInDeck(CardData cardData, int level)
		{
			if (CheckCanAddCard())
			{
				_inGameDeckListSO.cardDatas.Add(cardData);
				_userSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
			}
		}

		/// <summary>
		/// 인게임 카드 데이터를 새로고침한다
		/// </summary>
		/// <param name="cardData"></param>
		public void ChangeCardInInGameSaveData(CardData cardData)
		{
			CardSaveData cardSaveData = _userSaveData._ingameSaveDatas.Find(x => x._cardNamingType == cardData._cardNamingType);
			if(cardSaveData == null)
			{
				return;
			}
			CardSaveData changeSaveData = CardSaveData.CopyDataToCardData(cardData);
			cardSaveData._unitType = changeSaveData._unitType;
			cardSaveData._strategicType = changeSaveData._strategicType;
			cardSaveData._skinType = changeSaveData._skinType;
			cardSaveData._level = changeSaveData._level;
			cardSaveData._count = changeSaveData._count;
			cardSaveData._cardType = changeSaveData._cardType;
			cardSaveData._cardNamingType = changeSaveData._cardNamingType;
			cardSaveData.stickerType = changeSaveData.stickerType;
		}

		/// <summary>
		/// 덱에 카드를 해제한다
		/// </summary>
		public void RemoveCardInDeck(CardNamingType cardNamingType)
		{
			_inGameDeckListSO.cardDatas.RemoveAt(_inGameDeckListSO.cardDatas.FindIndex(x => x._cardNamingType == cardNamingType));
			_userSaveData._ingameSaveDatas.RemoveAt(_userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
		}

		/// <summary>
		/// 카드를 추가할 수 있는지 체크
		/// </summary>
		public bool CheckCanAddCard()
		{
			if (_inGameDeckListSO.cardDatas.Count == 10)
			{
				_warrningComponent.SetWarrning("장착 카드는 10장을 넘어갈 수 없습니다");
				return false;
			}
			return true;
		}
		/// <summary>
		/// 게임을 할 수 있는지 체크
		/// </summary>
		public bool CheckCanPlayGame()
		{
			if (_inGameDeckListSO.cardDatas.Count < 2)
			{
				_warrningComponent.SetWarrning("장착 카드는 2장 이상이어야 합니다");
				return false;
			}
			return true;
		}

		/// <summary>
		/// 카드가 이미 장착되어 있는지 확인한다
		/// </summary>
		/// <returns></returns>
		public bool ReturnAlreadyEquipCard(CardNamingType cardNamingType)
		{
			if (_inGameDeckListSO.cardDatas.Find(x => x._cardNamingType == cardNamingType) != null)
			{
				return true;
			}
			return false;
		}
	}
}