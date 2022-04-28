using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

namespace Main.Deck
{
    public class DeckSettingComponent : MonoBehaviour
    {
        [SerializeField]
        private UserDeckDataComponent userDeckData;
        [SerializeField]
        private GameObject cardPrefab;
        [SerializeField]
        private GameObject cardDescription;
        [SerializeField]
        private GameObject deckScroll;

        public List<GameObject> deckCards = new List<GameObject>();
        private void Awake()
        {
        }
        private void Start()
        {
            SetDeck();
            EventManager.StartListening(EventsType.ActiveDeck, UpdateDeck);
        }

        /// <summary>
        /// 플레이어 덱에 카드 세팅 
        /// </summary>
        [ContextMenu("SetDeck")]
        public void SetDeck()
        {
            userDeckData.SetCardData();
            for (int i = 0; i < userDeckData.deckList.cardDatas.Count; i++)
            {
                GameObject cardObj = CreateCard();
                //cardObj.GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
                cardObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(EventsType.DeckSetting, ButtonType.cardDescription);
                });
                deckCards.Add(cardObj);
            }
        }
        /// <summary>
        /// 게임 실행중 덱 업데이트 (카드 추가)
        /// </summary>
        public void UpdateDeck() //나중에 카드 추가 기능만들 때 이벤트로 같이 넘겨줄거야 
        {
            userDeckData.SetCardData();
            for (int i = 0; i < deckCards.Count; i++)
            {
                if (deckCards[i] == null) deckCards[i] = CreateCard();
                deckCards[i].GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
            }
        }
        /// <summary>
        /// 덱에 카드 생성 
        /// </summary>
        /// <returns></returns>
        public GameObject CreateCard()
        {
            GameObject cardObj = Instantiate(cardPrefab, deckScroll.transform.GetChild(0).GetChild(0), false);
            return cardObj;
        }

        /// <summary>
        /// 카드 정보 패널 활성화
        /// </summary>
        private void OnActiveCardInfoPn()
        {
            cardDescription.SetActive(!cardDescription.activeSelf);
        }
    }
}