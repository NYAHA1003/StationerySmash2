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
        private UserDeckDataComponent _userDeckData; //유저 데이터 컴포넌트
        [SerializeField]
        private GameObject _cardPrefab; //카드 UI 프리펩
        [SerializeField]
        private GameObject _haveDeckScroll; //보유 카드 스크롤
        [SerializeField]
        private GameObject _equipDeckScroll; //장착 카드 스크롤

        private Transform _haveCardParent = null;
        private Transform _equipCardParent = null;

        public List<GameObject> _haveDeckCards = new List<GameObject>();
        public List<GameObject> _equipDeckCards = new List<GameObject>();
        private void Start()
        {
            _haveCardParent = _haveDeckScroll.transform.GetChild(0).GetChild(0);
            _equipCardParent = _equipDeckScroll.transform.GetChild(0).GetChild(0);

            UpdateHaveAndEquipDeck();

            EventManager.StartListening(EventsType.ActiveDeck, UpdateDeck);
            EventManager.StartListening(EventsType.UpdateHaveAndEquipDeck, UpdateHaveAndEquipDeck);
        }

        /// <summary>
        /// 보유 덱과 장착 덱을 새로고침한다
        /// </summary>
        public void UpdateHaveAndEquipDeck()
        {
            AllFalseEquipCard();
            AllFalseHaveCard();
            SetHaveDeck();
            SetEquipDeck();
        }

        /// <summary>
        /// 플레이어 덱에 카드 세팅 
        /// </summary>
        public void SetHaveDeck()
        {
            _userDeckData.SetCardData();
            for (int i = 0; i < _userDeckData.deckList.cardDatas.Count; i++)
            {
                GameObject cardObj = PoolHaveCard();
                Button cardButton = cardObj.GetComponent<Button>();
                cardObj.GetComponent<DeckCard>().SetCard(_userDeckData.deckList.cardDatas[i]);
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(EventsType.ActiveCardDescription, cardObj.GetComponent<DeckCard>());
                    EventManager.TriggerEvent(EventsType.DeckSetting, ButtonType.cardDescription);
                    
                });
                _haveDeckCards.Add(cardObj);
            }
        }

        /// <summary>
        /// 장착 카드 세팅
        /// </summary>
        public void SetEquipDeck()
        {
            _userDeckData.SetCardData();
            for (int i = 0; i < _userDeckData.inGameDeckList.cardDatas.Count; i++)
            {
                GameObject cardObj = PoolEquipCard();
                Button cardButton = cardObj.GetComponent<Button>();
                cardObj.GetComponent<DeckCard>().SetCard(_userDeckData.inGameDeckList.cardDatas[i]);
                cardButton.onClick.RemoveAllListeners();
                cardButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(EventsType.ActiveCardDescription, cardObj.GetComponent<DeckCard>());
                    EventManager.TriggerEvent(EventsType.DeckSetting, ButtonType.cardDescription);
                    
                });
                _equipDeckCards.Add(cardObj);
            }
        }

        /// <summary>
        /// 게임 실행중 덱 업데이트 (카드 추가)
        /// </summary>
        public void UpdateDeck() //나중에 카드 추가 기능만들 때 이벤트로 같이 넘겨줄거야 
        {
            _userDeckData.SetCardData();
            for (int i = 0; i < _haveDeckCards.Count; i++)
            {
                if (_haveDeckCards[i] == null)
				{
                    _haveDeckCards[i] = PoolHaveCard();
				}
                _haveDeckCards[i].GetComponent<DeckCard>().SetCard(_userDeckData.deckList.cardDatas[i]);
            }
        }
        /// <summary>
        /// 보유덱에 사용할 카드 오브젝트 가져오기 
        /// </summary>
        /// <returns></returns>
        public GameObject PoolHaveCard()
        {
            GameObject cardObj = null;

            int count = _haveCardParent.childCount;


            for (int i = 0; i < count; i++)
            {
                if (_haveCardParent.childCount > i && !_haveCardParent.GetChild(i).gameObject.activeSelf)
                {
                    cardObj = _haveCardParent.GetChild(i).gameObject;
                    break;
                }
            }

            if(cardObj == null)
            {
                cardObj = Instantiate(_cardPrefab, _haveCardParent, false);
            }

            cardObj.SetActive(true);

            return cardObj;
        }
        /// <summary>
        /// 보유덱에 사용할 카드 오브젝트 가져오기 
        /// </summary>
        /// <returns></returns>
        public GameObject PoolEquipCard()
        {
            GameObject cardObj = null;

            int count = _equipCardParent.childCount;


            for (int i = 0; i < count; i++)
            {
                if (_equipCardParent.childCount > i && !_equipCardParent.GetChild(i).gameObject.activeSelf)
                {
                    cardObj = _equipCardParent.GetChild(i).gameObject;
                    break;
                }
            }

            if (cardObj == null)
            {
                cardObj = Instantiate(_cardPrefab, _equipCardParent, false);
            }

            cardObj.SetActive(true);

            return cardObj;
        }

        /// <summary>
        /// 보유덱에 있는 모든 카드 끄기
        /// </summary>
        public void AllFalseHaveCard()
        {
            int count = _haveCardParent.childCount;

            for (int i = 0; i < count; i++)
            {
                _haveCardParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 장착덱에 있는 모든 카드 끄기
        /// </summary>
        public void AllFalseEquipCard()
        {
            int count = _equipCardParent.childCount;

            for (int i = 0; i < count; i++)
            {
                _equipCardParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}