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
        public SaveDataSO saveData; //���̺� ������(ī�� ����, ��������) 
        public CardDeckSO standardcardDeck; //���� ������ 
        public CardDeckSO deckList; //ī�� ������
        public CardDeckSO inGameDeckList; //ī�� ������

        /// <summary>
        /// ī�嵦 �����͸� �������ݴϴ� 
        /// </summary>
        [ContextMenu("SetCardData")]
        public void SetCardData()
        {
            //���������� json���� �о��
            JsonToData();

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
            int count = saveData.userSaveData._unitSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = saveData.userSaveData._unitSaveDatas[i];
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
            int count = saveData.userSaveData._ingameSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = saveData.userSaveData._ingameSaveDatas[i];
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
            saveData.userSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
        }
        /// <summary>
        /// ���� ī�带 �����Ѵ�
        /// </summary>
        public void RemoveCardInDeck(CardNamingType cardNamingType)
        {
            inGameDeckList.cardDatas.RemoveAt(inGameDeckList.cardDatas.FindIndex(x => x.skinData._cardNamingType == cardNamingType));
            saveData.userSaveData._ingameSaveDatas.RemoveAt(saveData.userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
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

        /// <summary>
        /// ���� ���̺� �����͸� Jsonȭ ���� �����Ѵ�
        /// </summary>
        [ContextMenu("DataToJson")]
        public void DataToJson()
        {
            //�����͸� json���� ��ȯ
            string json = JsonUtility.ToJson(saveData.userSaveData, true);

            //json�� �����Ѵ� ���Ϸ� ����Ƽ���������ʿ�
            string fileName = "saveData";
            string path = Application.dataPath + "/" + fileName + ".json";
            File.WriteAllText(path, json);
        }


        /// <summary>
        /// jsonȭ ��Ų 
        /// </summary>
        [ContextMenu("JsonToData")]
        public void JsonToData()
        {
            //json ���� �ҷ�����
            try
            {
                string path = File.ReadAllText(Application.dataPath + "/saveData.json");


                //���������Ϳ� ����
                saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
            }
            catch
			{
                //���� �߸� ������ ����
			}
        }
    }
}