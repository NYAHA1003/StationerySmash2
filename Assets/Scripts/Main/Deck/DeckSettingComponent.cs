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
        /// �÷��̾� ���� ī�� ���� 
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
        /// ���� ������ �� ������Ʈ (ī�� �߰�)
        /// </summary>
        public void UpdateDeck() //���߿� ī�� �߰� ��ɸ��� �� �̺�Ʈ�� ���� �Ѱ��ٰž� 
        {
            userDeckData.SetCardData();
            for (int i = 0; i < deckCards.Count; i++)
            {
                if (deckCards[i] == null) deckCards[i] = CreateCard();
                deckCards[i].GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
            }
        }
        /// <summary>
        /// ���� ī�� ���� 
        /// </summary>
        /// <returns></returns>
        public GameObject CreateCard()
        {
            GameObject cardObj = Instantiate(cardPrefab, deckScroll.transform.GetChild(0).GetChild(0), false);
            return cardObj;
        }

        /// <summary>
        /// ī�� ���� �г� Ȱ��ȭ
        /// </summary>
        private void OnActiveCardInfoPn()
        {
            cardDescription.SetActive(!cardDescription.activeSelf);
        }
    }
}