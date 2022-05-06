using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Main.Deck;
using Utill.Data;

namespace Main.Deck
{
    public class UserDeckDataComponent : MonoBehaviour
    {
        //ī�� ����
        public CardDeckSO _standardcardDeck; //���� ī�� ������ 
        public CardDeckSO _deckList; //���� ī�� ������
        public CardDeckSO _inGameDeckList; //���� ī�� ������
        //���� ����
        public PencilCaseDataListSO _standardPCDataSO; //���� ���� �����͵�
        public PencilCaseDataListSO _havePCDataSO; //���� ���� �����͵�
        public PencilCaseDataSO _inGamePCDataSO; //���� ���� ������

        [SerializeField]
        private WarrningComponent _warrningComponent; //��� ������Ʈ

        private UserSaveData _userSaveData; //���� ������

        /// <summary>
        /// ī�嵦 �����͸� �������ݴϴ� 
        /// </summary>
        public void SetCardData()
        {
            //���̺� �������� ���� ���� �����͸� �����´�
            _userSaveData = SaveManager._instance._saveData.userSaveData;
            SaveManager._instance.DeliverDataToObserver();

            //ī�� ������ �ʱ�ȭ
            SetDeckCardList();
            SetIngameCardList();
        }

        /// <summary>
        /// ���� ī�� ������ ����
        /// </summary>
        public void SetDeckCardList()
        {
            _deckList.cardDatas.Clear();
            int count = _userSaveData._unitSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = _userSaveData._unitSaveDatas[i];
                //������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
                CardData cardDataobj = _standardcardDeck.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
                    _deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
                }
            }
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
                    //���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
                    _havePCDataSO._pencilCaseDataList.Add(pcData);
                    //_deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
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
            _inGameDeckList.cardDatas.Clear();
            int count = _userSaveData._ingameSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = _userSaveData._ingameSaveDatas[i];
                //������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
                CardData cardDataobj = _deckList.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
                    _inGameDeckList.cardDatas.Add(cardDataobj);
                }
            }
        }

        /// <summary>
        /// ���� ī�带 �߰��Ѵ�
        /// </summary>
        public void AddCardInDeck(CardData cardData, int level)
		{
            if(CheckCanAddCard())
			{
                _inGameDeckList.cardDatas.Add(cardData.DeepCopy(level, cardData.skinData._skinType));
                _userSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
			}
        }
        /// <summary>
        /// ���� ī�带 �����Ѵ�
        /// </summary>
        public void RemoveCardInDeck(CardNamingType cardNamingType)
        {
            _inGameDeckList.cardDatas.RemoveAt(_inGameDeckList.cardDatas.FindIndex(x => x.skinData._cardNamingType == cardNamingType));
            _userSaveData._ingameSaveDatas.RemoveAt(_userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
        }

        /// <summary>
        /// ī�带 �߰��� �� �ִ��� üũ
        /// </summary>
        public bool CheckCanAddCard()
        {
            if (_inGameDeckList.cardDatas.Count == 10)
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
            if (_inGameDeckList.cardDatas.Count < 2)
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
            if(_inGameDeckList.cardDatas.Find(x => x.skinData._cardNamingType == cardNamingType) != null)
			{
                return true;
            }
            return false;
        }
    }
}