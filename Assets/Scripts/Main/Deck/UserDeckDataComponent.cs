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
		//ī�� ����
		public CardDeckSO _standardcardDeckSO; //���� ī�� ������ 
		public CardDeckSO _haveDeckListSO; //���� ī�� ������
		public CardDeckSO _inGameDeckListSO; //���� ī�� ������

		//���� ����
		public PencilCaseDataListSO _standardPCDataSO; //���� ���� �����͵�
		public PencilCaseDataListSO _havePCDataSO; //���� ���� �����͵�
		public PencilCaseDataSO _inGamePCDataSO; //���� ���� ������

		//��ƼĿ ����
		public StickerDataSO _standardStickerDataSO; //���� ��ƼĿ �����͵�
		public StickerDataSO _haveStickerDataSO; //���� ��ƼĿ �����͵�

		//���� ����
		public BadgeListSO _standardBadgeDataSO; //���� ���� �����͵�
		public BadgeListSO _haveBadgeDataSO; //���� ���� �����͵�

		//��Ų ����
		public SkinListSO _skinListSO; //��Ų �����͵�

		//������
		[SerializeField]
		private PresetSaveDataSO _presetDataSO1 = null;
		[SerializeField]
		private PresetSaveDataSO _presetDataSO2 = null;
		[SerializeField]
		private PresetSaveDataSO _presetDataSO3 = null;

		[SerializeField]
		private WarrningComponent _warrningComponent; //��� ������Ʈ

		private UserSaveData _userSaveData; //���� ������

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
		/// ī�嵦 �����͸� �������ݴϴ� 
		/// </summary>
		public void SetCardData()
		{
			//ī�� ������ �ʱ�ȭ
			SetHaveDeckCardList();
			SetIngameCardList();

			//��ƼĿ ������ �ʱ�ȭ
			SetStickerDataList();

		}


		/// <summary>
		/// ���� ������ ����
		/// </summary>
		public void SetPencilCaseData()
		{
			SetPencilCaseList();
			SetInGamePencilCase(_havePCDataSO._pencilCaseDataList.Find(x => x._pencilCaseType == _userSaveData._currentPencilCaseType));

			//���� ������ �ʱ�ȭ
			SetBadgeDataList();
		}


		/// <summary>
		/// ������ ����
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
		/// ���� ī�� ������ ����
		/// </summary>
		public void SetHaveDeckCardList()
		{
			_haveDeckListSO.cardDatas.Clear();
			int count = _userSaveData._haveCardSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = _userSaveData._haveCardSaveDatas[i];
				//���� ī�� Ÿ�� ã��
				CardData cardDataobj = _standardcardDeckSO.cardDatas.Find(x => x._cardNamingType == saveDataobj._cardNamingType);

				if (cardDataobj != null)
				{
					//���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
					SkinData skinData = _skinListSO._cardNamingSkins.Find(x => x._cardNamingType == saveDataobj._cardNamingType)._skinDatas.Find(x => x._skinType == saveDataobj._skinType);
					CardData cardData = cardDataobj.DeepCopy();
					cardData.level = saveDataobj._level;
					cardData._skinData = skinData;
					_haveDeckListSO.cardDatas.Add(cardData);
				}
			}
		}

		/// <summary>
		/// ������ ������ �����Ѵ�
		/// </summary>
		public void HaveDeckSortABC()
		{
			var list = _haveDeckListSO.cardDatas.OrderBy(x => x.card_Name);
			_haveDeckListSO.cardDatas = list.ToList<CardData>();
		}

		/// <summary>
		/// �ڽ�Ʈ ������ �����Ѵ�
		/// </summary>
		public void HaveDeckSortCost()
		{
			var list = _haveDeckListSO.cardDatas.OrderBy(x => x.card_Cost);
			_haveDeckListSO.cardDatas = list.ToList<CardData>();
		}

		/// <summary>
		/// ���� ���� ����
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
		/// �ΰ��� ���� ����
		/// </summary>
		public void SetInGamePencilCase(PencilCaseData pencilCaseData)
		{
			_inGamePCDataSO._pencilCaseData = pencilCaseData;
		}

		/// <summary>
		/// �ΰ��� ī�� �� ����
		/// </summary>
		public void SetIngameCardList()
		{
			_inGameDeckListSO.cardDatas.Clear();
			int count = _userSaveData._ingameSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = _userSaveData._ingameSaveDatas[i];
				//������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
				CardData cardDataobj = _haveDeckListSO.cardDatas.Find(x => x._cardNamingType == saveDataobj._cardNamingType);

				if (cardDataobj != null)
				{
					//���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
					_inGameDeckListSO.cardDatas.Add(cardDataobj);
				}
			}
		}

		/// <summary>
		/// ���� ��ƼĿ �����͸� �����Ѵ�
		/// </summary>
		public void SetStickerDataList()
		{
			_haveStickerDataSO.GetStickerDataList().Clear();

			int count = _userSaveData._haveStickerList.Count;
			int categoryCount = _standardStickerDataSO.GetStickerDataList().Count;

			//�� ��ƼĿ ����Ʈ ����
			for (int i = 0; i < categoryCount; i++)
			{
				//��ƼĿ ����Ʈ
				var _stickerList = _standardStickerDataSO.GetStickerDataList()[i];
				//�� ��ƼĿ ����Ʈ ����
				_haveStickerDataSO.GetStickerDataList().Add(_stickerList.CopyEmptryList());
			}

			//������ �ִ� ��ƼĿ �˸´� ��ƼĿ ������ ã��
			for (int i = 0; i < categoryCount; i++)
			{
				//��ƼĿ ����Ʈ
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
		/// ���� ���� �����͸� �����Ѵ�
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
		/// ���� ī�带 �߰��Ѵ�
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
		/// �ΰ��� ī�� �����͸� ���ΰ�ħ�Ѵ�
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
		/// ���� ī�带 �����Ѵ�
		/// </summary>
		public void RemoveCardInDeck(CardNamingType cardNamingType)
		{
			_inGameDeckListSO.cardDatas.RemoveAt(_inGameDeckListSO.cardDatas.FindIndex(x => x._cardNamingType == cardNamingType));
			_userSaveData._ingameSaveDatas.RemoveAt(_userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
		}

		/// <summary>
		/// ī�带 �߰��� �� �ִ��� üũ
		/// </summary>
		public bool CheckCanAddCard()
		{
			if (_inGameDeckListSO.cardDatas.Count == 10)
			{
				_warrningComponent.SetWarrning("���� ī��� 10���� �Ѿ �� �����ϴ�");
				return false;
			}
			return true;
		}
		/// <summary>
		/// ������ �� �� �ִ��� üũ
		/// </summary>
		public bool CheckCanPlayGame()
		{
			if (_inGameDeckListSO.cardDatas.Count < 2)
			{
				_warrningComponent.SetWarrning("���� ī��� 2�� �̻��̾�� �մϴ�");
				return false;
			}
			return true;
		}

		/// <summary>
		/// ī�尡 �̹� �����Ǿ� �ִ��� Ȯ���Ѵ�
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