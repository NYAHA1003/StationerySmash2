using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Event;

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

        private Transform cardParent = null;

        public List<GameObject> deckCards = new List<GameObject>();
        private void Start()
        {
            cardParent = deckScroll.transform.GetChild(0).GetChild(0);
            SetHaveDeck();
            EventManager.StartListening(EventsType.ActiveDeck, UpdateDeck);
        }

        /// <summary>
        /// 플레이어 덱에 카드 세팅 
        /// </summary>
        [ContextMenu("SetDeck")]
        public void SetHaveDeck()
        {
            userDeckData.SetCardData();
            AllFalseHaveCard();
            for (int i = 0; i < userDeckData.deckList.cardDatas.Count; i++)
            {
                GameObject cardObj = PoolHaveCard();
                cardObj.GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
                cardObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(EventsType.ActiveCardDescription, cardObj.GetComponent<DeckCard>());
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
                if (deckCards[i] == null) deckCards[i] = PoolHaveCard();
                deckCards[i].GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
            }
        }
        /// <summary>
        /// 보유덱에 카드 생성 
        /// </summary>
        /// <returns></returns>
        public GameObject PoolHaveCard()
        {
            GameObject cardObj = null;

            int count = cardParent.childCount;


            for (int i = 0; i < count; i++)
            {
                if (cardParent.childCount > i && !cardParent.GetChild(i).gameObject.activeSelf)
                {
                    cardObj = cardParent.GetChild(i).gameObject;
                    break;
                }
            }

            if(cardObj == null)
            {
                cardObj = Instantiate(cardPrefab, cardParent, false);
            }

            cardObj.SetActive(true);

            return cardObj;
        }

        /// <summary>
        /// 보유덱에 있는 모든 카드 끄기
        /// </summary>
        public void AllFalseHaveCard()
        {
            int count = cardParent.childCount;


            for (int i = 0; i < count; i++)
            {
                cardParent.GetChild(i).gameObject.SetActive(false);
            }
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