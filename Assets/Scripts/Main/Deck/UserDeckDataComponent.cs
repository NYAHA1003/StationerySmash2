using System.Collections;
using System.Collections.Generic;
using System.IO;
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

		public void ChangePreset(int index)
		{
			switch (index)
			{
				case 0:
					_userSaveData._ingameSaveDatas = _presetDataSO1._ingameSaveDatas;
					_userSaveData._currentPencilCaseType = _presetDataSO1._pencilCaseData._pencilCaseType;
					SetInGamePencilCase(_presetDataSO1._pencilCaseData);
					break;
				case 1:
					_userSaveData._ingameSaveDatas = _presetDataSO2._ingameSaveDatas;
					_userSaveData._currentPencilCaseType = _presetDataSO2._pencilCaseData._pencilCaseType;
					SetInGamePencilCase(_presetDataSO2._pencilCaseData);
					break;
				case 2:
					_userSaveData._ingameSaveDatas = _presetDataSO3._ingameSaveDatas;
					_userSaveData._currentPencilCaseType = _presetDataSO3._pencilCaseData._pencilCaseType;
					SetInGamePencilCase(_presetDataSO3._pencilCaseData);
					break;
			}
			_userSaveData._setPrestIndex = index;
		}

		/// <summary>
		/// ���� ī�� ������ ����
		/// </summary>
		public void SetHaveDeckCardList()
		{
			_haveDeckListSO.cardDatas.Clear();
			int count = _userSaveData._unitSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = _userSaveData._unitSaveDatas[i];
				//���� ī�� Ÿ�� ã��
				CardData cardDataobj = _standardcardDeckSO.cardDatas.Find(x => x._cardNamingType == saveDataobj._cardNamingType);

				if (cardDataobj != null)
				{
					//���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
					_haveDeckListSO.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
				}
			}
		}

		/// <summary>
		/// ���� ������ ����
		/// </summary>
		public void SetPencilCaseData()
		{
			SetPencilCaseList();
			SetInGamePencilCase(_havePCDataSO._pencilCaseDataList.Find(x => x._pencilCaseType == _userSaveData._currentPencilCaseType));
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
			_haveStickerDataSO._stickerDataLists.Clear();

			int count = _userSaveData._haveStickerList.Count;
			int categoryCount = _standardStickerDataSO._stickerDataLists.Count;

			//�� ��ƼĿ ����Ʈ ����
			for (int i = 0; i < categoryCount; i++)
			{
				//��ƼĿ ����Ʈ
				var _stickerList = _standardStickerDataSO._stickerDataLists[i];
				//�� ��ƼĿ ����Ʈ ����
				_haveStickerDataSO._stickerDataLists.Add(_stickerList.CopyEmptryList());
			}

			//������ �ִ� ��ƼĿ �˸´� ��ƼĿ ������ ã��
			for (int i = 0; i < categoryCount; i++)
			{
				//��ƼĿ ����Ʈ
				var _stickerList = _standardStickerDataSO._stickerDataLists[i];

				for (int j = 0; j < _userSaveData._haveStickerList.Count; j++)
				{
					StickerSaveData stickerSaveData = _userSaveData._haveStickerList[j];
					StickerData addStickerData = _stickerList._stickerDatas.Find(x => x._stickerType == stickerSaveData._stickerType);
					if(addStickerData != null)
					{
						_haveStickerDataSO._stickerDataLists[i]._stickerDatas.Add(addStickerData.DeepCopyStickerData(stickerSaveData));
					}
				}
			}
		}

		/// <summary>
		/// ���� ī�带 �߰��Ѵ�
		/// </summary>
		public void AddCardInDeck(CardData cardData, int level)
		{
			if (CheckCanAddCard())
			{
				_inGameDeckListSO.cardDatas.Add(cardData.DeepCopy(level, cardData.skinData._skinType));
				_userSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
			}
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