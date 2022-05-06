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
        //카드 관련
        public CardDeckSO _standardcardDeck; //기준 카드 데이터 
        public CardDeckSO _deckList; //보유 카드 데이터
        public CardDeckSO _inGameDeckList; //장착 카드 데이터
        //필통 관련
        public PencilCaseDataListSO _standardPCDataSO; //기준 필통 데이터들
        public PencilCaseDataListSO _havePCDataSO; //보유 필통 데이터들
        public PencilCaseDataSO _inGamePCDataSO; //장착 필통 데이터

        [SerializeField]
        private WarrningComponent _warrningComponent; //경고 컴포넌트

        private UserSaveData _userSaveData; //유저 데이터

        /// <summary>
        /// 카드덱 데이터를 세팅해줍니다 
        /// </summary>
        public void SetCardData()
        {
            //세이브 데이터의 유저 저장 데이터를 가져온다
            _userSaveData = SaveManager._instance._saveData.userSaveData;
            SaveManager._instance.DeliverDataToObserver();

            //카드 데이터 초기화
            SetDeckCardList();
            SetIngameCardList();
        }

        /// <summary>
        /// 보유 카드 데이터 설정
        /// </summary>
        public void SetDeckCardList()
        {
            _deckList.cardDatas.Clear();
            int count = _userSaveData._unitSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = _userSaveData._unitSaveDatas[i];
                //세가지 타입이 세이브데이터와 모두 같은 기준 데이터 찾기
                CardData cardDataobj = _standardcardDeck.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
                    _deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
                }
            }
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
                    //세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
                    _havePCDataSO._pencilCaseDataList.Add(pcData);
                    //_deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level, saveDataobj._skinType));
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
            _inGameDeckList.cardDatas.Clear();
            int count = _userSaveData._ingameSaveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardSaveData saveDataobj = _userSaveData._ingameSaveDatas[i];
                //세가지 타입이 세이브데이터와 모두 같은 기준 데이터 찾기
                CardData cardDataobj = _deckList.cardDatas.Find(x => x.skinData._cardNamingType == saveDataobj._cardNamingType);

                if (cardDataobj != null)
                {
                    //세이브데이터의 레벨만큼 수치를 변경하고 새로운 카드데이터로 만들어 받아 덱리스트에 추가
                    _inGameDeckList.cardDatas.Add(cardDataobj);
                }
            }
        }

        /// <summary>
        /// 덱에 카드를 추가한다
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
        /// 덱에 카드를 해제한다
        /// </summary>
        public void RemoveCardInDeck(CardNamingType cardNamingType)
        {
            _inGameDeckList.cardDatas.RemoveAt(_inGameDeckList.cardDatas.FindIndex(x => x.skinData._cardNamingType == cardNamingType));
            _userSaveData._ingameSaveDatas.RemoveAt(_userSaveData._ingameSaveDatas.FindIndex(x => x._cardNamingType == cardNamingType));
        }

        /// <summary>
        /// 카드를 추가할 수 있는지 체크
        /// </summary>
        public bool CheckCanAddCard()
        {
            if (_inGameDeckList.cardDatas.Count == 10)
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
            if (_inGameDeckList.cardDatas.Count < 2)
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
            if(_inGameDeckList.cardDatas.Find(x => x.skinData._cardNamingType == cardNamingType) != null)
			{
                return true;
            }
            return false;
        }
    }
}