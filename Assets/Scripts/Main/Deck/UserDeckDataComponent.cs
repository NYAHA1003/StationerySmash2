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
		//ī�� ����
		public CardDeckSO _standardcardDeckSO; //���� ī�� ������ 
		public CardDeckSO _haveDeckListSO; //���� ī�� ������
		public CardDeckSO _inGameDeckListSO; //���� ī�� ������

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

		private void Awake()
		{
			SaveManager._instance.SaveData.AddObserver(this);

			SetCardData();
			SetPencilCaseData();
			ChangePreset(UserSaveManagerSO.UserSaveData._setPrestIndex);
		}

		public void Notify()
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

		}


		/// <summary>
		/// ���� ������ ����
		/// </summary>
		public void SetPencilCaseData()
		{
			PencilCaseDataManagerSO.ResetPencilCaseList();
			PencilCaseDataManagerSO.SetInGamePencilCase(PencilCaseDataManagerSO.HavePencilCaseDataList.Find(x => x._pencilCaseType == UserSaveManagerSO.UserSaveData._currentPencilCaseType));
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

			UserSaveManagerSO.UserSaveData._ingameSaveDatas = presetSaveDataSO._ingameSaveDatas;
			UserSaveManagerSO.UserSaveData._currentPencilCaseType = presetSaveDataSO._pencilCaseData._pencilCaseType;

			PencilCaseData pencilCaseData = PencilCaseDataManagerSO.HavePencilCaseDataList.Find(x => x._pencilCaseType == presetSaveDataSO._pencilCaseData._pencilCaseType);
			PencilCaseData changePCData = null;

			changePCData = pencilCaseData.DeepCopyNoneBadge();
			for (int i = 0; i < presetSaveDataSO._pencilCaseData._badgeDatas.Count; i++)
			{
				changePCData._badgeDatas.Add(BadgeDataManagerSO.HaveBadgeDataList.Find(x => x._badgeType == presetSaveDataSO._pencilCaseData._badgeDatas[i]._BadgeType));
			}
			PencilCaseDataManagerSO.SetInGamePencilCase(changePCData);


			UserSaveManagerSO.UserSaveData._setPrestIndex = index;
		}

		/// <summary>
		/// ���� ī�� ������ ����
		/// </summary>
		public void SetHaveDeckCardList()
		{
			_haveDeckListSO.cardDatas.Clear();
			int count = UserSaveManagerSO.UserSaveData._haveCardSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = UserSaveManagerSO.UserSaveData._haveCardSaveDatas[i];
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
		/// �ΰ��� ī�� �� ����
		/// </summary>
		public void SetIngameCardList()
		{
			_inGameDeckListSO.cardDatas.Clear();
			int count = UserSaveManagerSO.UserSaveData._ingameSaveDatas.Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = UserSaveManagerSO.UserSaveData._ingameSaveDatas[i];
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
		/// ���� ī�带 �߰��Ѵ�
		/// </summary>
		public void AddCardInDeck(CardData cardData, int level)
		{
			if (CheckCanAddCard())
			{
				_inGameDeckListSO.cardDatas.Add(cardData);
				UserSaveManagerSO.UserSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
			}
		}

		/// <summary>
		/// �ΰ��� ī�� �����͸� ���ΰ�ħ�Ѵ�
		/// </summary>
		/// <param name="cardData"></param>
		public void ChangeCardInInGameSaveData(CardData cardData)
		{
			CardSaveData cardSaveData = UserSaveManagerSO.UserSaveData._ingameSaveDatas.Find(x => x._cardNamingType == cardData._cardNamingType);
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
			UserSaveManagerSO.UserSaveData._ingameSaveDatas.RemoveAt(UserSaveManagerSO.UserSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
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