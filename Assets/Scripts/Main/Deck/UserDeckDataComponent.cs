using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Main.Deck;
using Utill.Data;
using Utill.Tool;

namespace Main.Deck
{
	public class UserDeckDataComponent : MonoBehaviour, IUserData
	{
		//카드 관련
		public CardDeckSO _haveDeckListSO; //보유 카드 데이터
		public CardDeckSO _inGameDeckListSO; //장착 카드 데이터

		//스킨 관련
		public SkinListSO _skinListSO; //스킨 데이터들

		[SerializeField]
		private WarrningComponent _warrningComponent; //경고 컴포넌트

		private void Awake()
		{
			UserSaveManagerSO.AddObserver(this);

			ChangePreset(UserSaveManagerSO.UserSaveData._setPrestIndex);
			SetCardData();
			SetPencilCaseData();
		}

		public void Notify()
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

		}


		/// <summary>
		/// 필통 데이터 설정
		/// </summary>
		public void SetPencilCaseData()
		{
			PencilCaseDataManagerSO.ResetPencilCaseList();
			PencilCaseDataManagerSO.SetInGamePencilCase(UserSaveManagerSO.UserSaveData._currentPencilCaseType);
		}

		/// <summary>
		/// 프리셋 변경
		/// </summary>
		/// <param name="index"></param>
		public void ChangePreset(int index)
		{
			UserSaveManagerSO.ChangeIngameData(index);
			PencilCaseDataManagerSO.SetInGamePencilCase(UserSaveManagerSO.UserSaveData._currentPencilCaseType);
		}

		/// <summary>
		/// 보유 카드 데이터 설정
		/// </summary>
		public void SetHaveDeckCardList()
		{
			_haveDeckListSO.cardDatas.Clear();
			int count = UserSaveManagerSO.UserSaveData._haveCardSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = UserSaveManagerSO.UserSaveData._haveCardSaveDatas[i];
				//같은 카드 타입 찾기
				CardData cardDataobj = DeckDataManagerSO.StdDeckDataList.Find(x => x._cardNamingType == saveDataobj._cardNamingType);

				if (cardDataobj != null)
				{
					//세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
					var skinDatas = _skinListSO._cardNamingSkins.Find(x => x._cardNamingType == saveDataobj._cardNamingType)._skinDatas;
					SkinData skinData = skinDatas.Find(x => x._skinType == saveDataobj._skinType);
					CardData cardData = cardDataobj.DeepCopy();
					cardData._level = saveDataobj._level;
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
			var list = _haveDeckListSO.cardDatas.OrderBy(x => x._name);
			_haveDeckListSO.cardDatas = list.ToList<CardData>();
		}

		/// <summary>
		/// 코스트 순으로 정렬한다
		/// </summary>
		public void HaveDeckSortCost()
		{
			var list = _haveDeckListSO.cardDatas.OrderBy(x => x._cost);
			_haveDeckListSO.cardDatas = list.ToList<CardData>();
		}

		/// <summary>
		/// 인게임 카드 덱 설정
		/// </summary>
		public void SetIngameCardList()
		{
			_inGameDeckListSO.cardDatas.Clear();
			int count = UserSaveManagerSO.UserSaveData.GetIngameCardData().Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = UserSaveManagerSO.UserSaveData.GetIngameCardData()[i];
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
		/// 덱에 카드를 추가한다
		/// </summary>
		public void AddCardInDeck(CardData cardData, int level)
		{
			if (CheckCanAddCard())
			{
				_inGameDeckListSO.cardDatas.Add(cardData);
				UserSaveManagerSO.UserSaveData.AddIngameCardData(cardData._cardNamingType);
			}
		}

		/// <summary>
		/// 인게임 카드 데이터를 새로고침한다
		/// </summary>
		/// <param name="cardData"></param>
		public void ChangeCardInInGameSaveData(CardData cardData)
		{
			CardSaveData cardSaveData = UserSaveManagerSO.UserSaveData.GetIngameCardData().Find(x => x._cardNamingType == cardData._cardNamingType);
			if(cardSaveData == null)
			{
				return;
			}
			CardSaveData changeSaveData = CardSaveData.CopyDataToCardData(cardData);
			cardSaveData._skinType = changeSaveData._skinType;
			cardSaveData._level = changeSaveData._level;
			cardSaveData._count = changeSaveData._count;
			cardSaveData._cardNamingType = changeSaveData._cardNamingType;
			cardSaveData._stickerType = changeSaveData._stickerType;
		}

		/// <summary>
		/// 덱에 카드를 해제한다
		/// </summary>
		public void RemoveCardInDeck(CardNamingType cardNamingType)
		{
			_inGameDeckListSO.cardDatas.RemoveAt(_inGameDeckListSO.cardDatas.FindIndex(x => x._cardNamingType == cardNamingType));
			UserSaveManagerSO.UserSaveData.RemoveIngameCardData(cardNamingType);
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