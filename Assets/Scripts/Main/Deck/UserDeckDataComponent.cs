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
        public SaveDataSO saveData; //세이브 데이터(카드 레벨, 보유여부) 
        public CardDeckSO standardcardDeck; //기준 데이터 
        public CardDeckSO deckList; //카드 데이터
        public CardDeckSO inGameDeckList; //카드 데이터

        /// <summary>
        /// 카드덱 데이터를 세팅해줍니다 
        /// </summary>
        [ContextMenu("SetCardData")]
        public void SetCardData()
        {
            //레벨데이터 json파일 읽어옴
            JsonToData();

            //카드 데이터 초기화
            SetDeckCardList();
            SetIngameCardList();
        }

        /// <summary>
        /// 보유 카드 데이터 설정
        /// </summary>
        public void SetDeckCardList()
        {
            deckList.cardDatas.Clear();
            int count = saveData.userSaveData._unitSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = saveData.userSaveData._unitSaveDatas[i];
                //세가지 타입이 세이브데이터와 모두 같은 기준 데이터 찾기
                CardData cardDataobj = standardcardDeck.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
                    deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
                }
            }
        }

        /// <summary>
        /// 인게임 카드 덱 설정
        /// </summary>
        public void SetIngameCardList()
        {
            inGameDeckList.cardDatas.Clear();
            int count = saveData.userSaveData._ingameSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = saveData.userSaveData._ingameSaveDatas[i];
                //세가지 타입이 세이브데이터와 모두 같은 기준 데이터 찾기
                CardData cardDataobj = deckList.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
                    inGameDeckList.cardDatas.Add(cardDataobj);
                }
            }
        }

        /// <summary>
        /// 덱에 카드를 추가한다
        /// </summary>
        public void AddCardInDeck(CardData cardData, int level)
		{
            inGameDeckList.cardDatas.Add(cardData.DeepCopy(level, cardData.skinData._skinType));
            saveData.userSaveData._ingameSaveDatas.Add(CardSaveData.CopyDataToCardData(cardData));
        }
        /// <summary>
        /// 덱에 카드를 해제한다
        /// </summary>
        public void RemoveCardInDeck(CardNamingType cardNamingType)
        {
            inGameDeckList.cardDatas.RemoveAt(inGameDeckList.cardDatas.FindIndex(x => x.skinData._cardNamingType == cardNamingType));
            saveData.userSaveData._ingameSaveDatas.RemoveAt(saveData.userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
        }


        /// <summary>
        /// 카드가 이미 장착되어 있는지 확인한다
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
        /// 유저 세이브 데이터를 Json화 시켜 저장한다
        /// </summary>
        [ContextMenu("DataToJson")]
        public void DataToJson()
        {
            //데이터를 json으로 변환
            string json = JsonUtility.ToJson(saveData.userSaveData, true);

            //json을 저장한다 파일로 유니티에셋파일쪽에
            string fileName = "saveData";
            string path = Application.dataPath + "/" + fileName + ".json";
            File.WriteAllText(path, json);
        }


        /// <summary>
        /// json화 시킨 
        /// </summary>
        [ContextMenu("JsonToData")]
        public void JsonToData()
        {
            //json 파일 불러오기
            try
            {
                string path = File.ReadAllText(Application.dataPath + "/saveData.json");


                //유저데이터에 저장
                saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
            }
            catch
			{
                //에러 뜨면 파일이 없음
			}
        }
    }
}