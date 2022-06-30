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
		public CardDeckSO _inGameDeckListSO; //���� ī�� ������

		//��Ų ����
		public SkinListSO _skinListSO; //��Ų �����͵�

		[SerializeField]
		private WarrningComponent _warrningComponent; //��� ������Ʈ

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
		/// ī�嵦 �����͸� �������ݴϴ� 
		/// </summary>
		public void SetCardData()
		{
			SetIngameCardList();
		}


		/// <summary>
		/// ���� ������ ����
		/// </summary>
		public void SetPencilCaseData()
		{
			PencilCaseDataManagerSO.ResetPencilCaseList();
			PencilCaseDataManagerSO.SetInGamePencilCase(UserSaveManagerSO.UserSaveData._currentPencilCaseType);
		}

		/// <summary>
		/// ������ ����
		/// </summary>
		/// <param name="index"></param>
		public void ChangePreset(int index)
		{
			UserSaveManagerSO.ChangeIngameData(index);
			PencilCaseDataManagerSO.SetInGamePencilCase(UserSaveManagerSO.UserSaveData._currentPencilCaseType);
		}

		/// <summary>
		/// ������ ������ �����Ѵ�
		/// </summary>
		public void HaveDeckSortABC()
		{
		 	DeckDataManagerSO.HaveCardSortABC();
		}

		/// <summary>
		/// �ڽ�Ʈ ������ �����Ѵ�
		/// </summary>
		public void HaveDeckSortCost()
		{
			DeckDataManagerSO.HaveCardSortCost();
		}

		/// <summary>
		/// �ΰ��� ī�� �� ����
		/// </summary>
		public void SetIngameCardList()
		{
			_inGameDeckListSO.cardDatas.Clear();
			int count = UserSaveManagerSO.UserSaveData.GetIngameCardData().Count;
			for (int i = 0; i < count; i++)
			{
				CardSaveData saveDataobj = UserSaveManagerSO.UserSaveData.GetIngameCardData()[i];
				//������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
				CardData cardDataobj = DeckDataManagerSO.FindHaveCardData(saveDataobj._cardNamingType);

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
				UserSaveManagerSO.UserSaveData.AddIngameCardData(cardData._cardNamingType);
			}
		}

		/// <summary>
		/// �ΰ��� ī�� �����͸� ���ΰ�ħ�Ѵ�
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
		/// ���� ī�带 �����Ѵ�
		/// </summary>
		public void RemoveCardInDeck(CardNamingType cardNamingType)
		{
			_inGameDeckListSO.cardDatas.RemoveAt(_inGameDeckListSO.cardDatas.FindIndex(x => x._cardNamingType == cardNamingType));
			UserSaveManagerSO.UserSaveData.RemoveIngameCardData(cardNamingType);
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
			//if (_inGameDeckListSO.cardDatas.Count < 1)
			//{
			//	_warrningComponent.SetWarrning("���� ī��� 2�� �̻��̾�� �մϴ�");
			//	return false;
			//}
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