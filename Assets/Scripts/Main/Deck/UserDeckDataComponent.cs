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
        public CardDeckSO standardcardDeck; //���� ������ 
        public CardDeckSO deckList; //ī�� ������
        public CardDeckSO inGameDeckList; //ī�� ������

        private UserSaveData _userSaveData; //���� ������

        /// <summary>
        /// ī�嵦 �����͸� �������ݴϴ� 
        /// </summary>
        [ContextMenu("SetCardData")]
        public void SetCardData()
        {
            //���̺� �������� ���� ���� �����͸� �����´�
            _userSaveData = SaveManager._instance._saveData.userSaveData;

            //ī�� ������ �ʱ�ȭ
            SetDeckCardList();
            SetIngameCardList();
        }

        /// <summary>
        /// ���� ī�� ������ ����
        /// </summary>
        public void SetDeckCardList()
        {
            deckList.cardDatas.Clear();
            int count = _userSaveData._unitSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = _userSaveData._unitSaveDatas[i];
                //������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
                CardData cardDataobj = standardcardDeck.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
                    deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
                }
            }
        }

        /// <summary>
        /// �ΰ��� ī�� �� ����
        /// </summary>
        public void SetIngameCardList()
        {
            inGameDeckList.cardDatas.Clear();
            int count = _userSaveData._ingameSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = _userSaveData._ingameSaveDatas[i];
                //������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
                CardData cardDataobj = deckList.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
                    inGameDeckList.cardDatas.Add(cardDataobj);
                }
            }
        }

        /// <summary>
        /// ���� ī�带 �߰��Ѵ�
        /// </summary>
        public void AddCardInDeck(CardData cardData, int level)
		{
            inGameDeckList.cardDatas.Add(cardData.DeepCopy(level, cardData.skinData._skinType));
            _userSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
        }
        /// <summary>
        /// ���� ī�带 �����Ѵ�
        /// </summary>
        public void RemoveCardInDeck(CardNamingType cardNamingType)
        {
            inGameDeckList.cardDatas.RemoveAt(inGameDeckList.cardDatas.FindIndex(x => x.skinData._cardNamingType == cardNamingType));
            _userSaveData._ingameSaveDatas.RemoveAt(_userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
        }


        /// <summary>
        /// ī�尡 �̹� �����Ǿ� �ִ��� Ȯ���Ѵ�
        /// </summary>
        /// <returns></returns>
        public bool ReturnAlreadyEquipCard(CardNamingType cardNamingType)
		{
            if(inGameDeckList.cardDatas.Find(x => x.skinData._cardNamingType == cardNamingType) != null)
			{
                return true;
            }
            return false;
        }
    }
}