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

        public List<GameObject> HaveDeckCards => _haveDeckCards;
        public List<GameObject> EquipDeckCards => _equipDeckCards;
        public List<GameObject> HavePencilCaseCards => _havePencilCaseCards;


        [SerializeField]
        private SaveDataSO _saveDataSO = null;
        [SerializeField]
        private UserDeckDataComponent _userDeckData; //유저 데이터 컴포넌트
        [SerializeField]
        private GameObject _cardPrefab; //카드 UI 프리펩
        [SerializeField]
        private GameObject _pcCardPrefab; //필통 카드 UI 프리펩
        [SerializeField]
        private GameObject _haveDeckScroll; //보유 카드 스크롤
        [SerializeField]
        private GameObject _equipDeckScroll; //장착 카드 스크롤
        [SerializeField]
        private GameObject _havePCDeckScroll; //보유 필통 카드 스크롤
        [SerializeField]
        private GameObject _equipPCDeckScroll; //장착 필통 카드 스크롤
        [SerializeField]
        private GameObject _equipPencilCaseCards = null; //장착된 필통

        private Transform _haveCardParent = null; // 스크롤 content
        private Transform _equipCardParent = null; // 스크롤 content
        private Transform _havePCCardParent = null; // 스크롤 content 

        private List<GameObject> _haveDeckCards = new List<GameObject>();
        private List<GameObject> _equipDeckCards = new List<GameObject>();
        private List<GameObject> _havePencilCaseCards = new List<GameObject>();

        [SerializeField]
        private CardSaveDataSO _presetDataSO1 = null;
        [SerializeField]
        private CardSaveDataSO _presetDataSO2 = null;
        [SerializeField]
        private CardSaveDataSO _presetDataSO3 = null;

        [SerializeField]
        private Button _presetButton1 = null;
        [SerializeField]
        private Button _presetButton2 = null;
        [SerializeField]
        private Button _presetButton3 = null;

        private void Start()
        {
            _haveCardParent = _haveDeckScroll.transform.GetChild(0).GetChild(0);
            _equipCardParent = _equipDeckScroll.transform.GetChild(0).GetChild(0);
            _havePCCardParent = _havePCDeckScroll.transform.GetChild(0).GetChild(0);

            UpdateHaveAndEquipDeck();
            UpdateHaveAndEquipPCDeck(); 

            _presetButton1.onClick.AddListener(() => ChangePreset(0));
            _presetButton2.onClick.AddListener(() => ChangePreset(1));
            _presetButton3.onClick.AddListener(() => ChangePreset(2));

            EventManager.StartListening(EventsType.ActiveDeck, UpdateDeck);
            EventManager.StartListening(EventsType.UpdateHaveAndEquipDeck, UpdateHaveAndEquipDeck);
            EventManager.StartListening(EventsType.UpdateHaveAndEquipPCDeck, UpdateHaveAndEquipPCDeck);
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
        /// 보유 필통 덱과 장착 필통 덱을 새로고침한다
        /// </summary>
        public void UpdateHaveAndEquipPCDeck()
        {
            AllFalseHavePCCard();
            SetHavePCDeck(); 
        }

        /// <summary>
        /// 저장된 프리셋으로 변경
        /// </summary>
        public void ChangePreset(int index)
		{
            switch(index)
			{
                case 0:
                    _saveDataSO.userSaveData._ingameSaveDatas = _presetDataSO1._ingameSaveDatas;
                    break;
                case 1:
                    _saveDataSO.userSaveData._ingameSaveDatas = _presetDataSO2._ingameSaveDatas;
                    break;
                case 2:
                    _saveDataSO.userSaveData._ingameSaveDatas = _presetDataSO3._ingameSaveDatas;
                    break;
            }

            UpdateHaveAndEquipDeck();
        }

        /// <summary>
        /// 보유 덱에 카드 세팅 
        /// </summary>
        public void SetHaveDeck()
        {
            _userDeckData.SetCardData();
            for (int i = 0; i < _userDeckData._deckList.cardDatas.Count; i++)
            {
                GameObject cardObj = PoolHaveCard();
                Button cardButton = cardObj.GetComponent<Button>();
                cardObj.GetComponent<DeckCard>().SetCard(_userDeckData._deckList.cardDatas[i]);
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(EventsType.ActiveCardDescription, cardObj.GetComponent<DeckCard>());
                    EventManager.TriggerEvent(EventsType.DeckSetting, ButtonType.cardDescription);
                    
                });
            }
        }

        /// <summary>
        /// 장착 카드 세팅
        /// </summary>
        public void SetEquipDeck()
        {
            _userDeckData.SetCardData();
            for (int i = 0; i < _userDeckData._inGameDeckList.cardDatas.Count; i++)
            {
                GameObject cardObj = PoolEquipCard();
                Button cardButton = cardObj.GetComponent<Button>();
                cardObj.GetComponent<DeckCard>().SetCard(_userDeckData._inGameDeckList.cardDatas[i]);
                cardButton.onClick.RemoveAllListeners();
                cardButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(EventsType.ActiveCardDescription, cardObj.GetComponent<DeckCard>());
                    EventManager.TriggerEvent(EventsType.DeckSetting, ButtonType.cardDescription);
                    
                });
            }
        }

        /// <summary>
        /// 보유 필통 덱에 카드 세팅 
        /// </summary>
        public void SetHavePCDeck()
        {
            _userDeckData.SetPencilCaseList();
            for (int i = 0; i < _userDeckData._havePCDataSO._pencilCaseDataList.Count; i++)
            {
                GameObject cardObj = PoolHavePCCard();
                Button cardButton = cardObj.GetComponent<Button>();
                cardObj.GetComponent<PencilCaseCard>().SetPencilCaseData(_userDeckData._havePCDataSO._pencilCaseDataList[i]);
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() =>
                {
                   // EventManager.TriggerEvent(EventsType.ActiveCardDescription, cardObj.GetComponent<DeckCard>());
                   // EventManager.TriggerEvent(EventsType.DeckSetting, ButtonType.cardDescription);

                });
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
                _haveDeckCards[i].GetComponent<DeckCard>().SetCard(_userDeckData._deckList.cardDatas[i]);
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
                if (!_haveCardParent.GetChild(i).gameObject.activeSelf)
                {
                    cardObj = _haveCardParent.GetChild(i).gameObject;
                    break;
                }
            }

            if(cardObj == null)
            {
                cardObj = Instantiate(_cardPrefab, _haveCardParent, false);
                _haveDeckCards.Add(cardObj);
            }

            cardObj.SetActive(true);

            return cardObj;
        }
        /// <summary>
        /// 장착덱에 사용할 카드 오브젝트 가져오기 
        /// </summary>
        /// <returns></returns>
        public GameObject PoolEquipCard()
        {
            GameObject cardObj = null;

            int count = _equipCardParent.childCount;


            for (int i = 0; i < count; i++)
            {
                if (!_equipCardParent.GetChild(i).gameObject.activeSelf)
                {
                    cardObj = _equipCardParent.GetChild(i).gameObject;
                    break;
                }
            }

            if (cardObj == null)
            {
                cardObj = Instantiate(_cardPrefab, _equipCardParent, false);
                _equipDeckCards.Add(cardObj);
            }

            cardObj.SetActive(true);

            return cardObj;
        }


        /// <summary>
        /// 보유 필통덱에 사용할 필통카드 오브젝트 가져오기 
        /// </summary>
        /// <returns></returns>
        public GameObject PoolHavePCCard()
        {
            GameObject pcCardObj = null;

            int count = _havePCCardParent.childCount;


            for (int i = 0; i < count; i++)
            {
                if (!_havePCCardParent.GetChild(i).gameObject.activeSelf)
                {
                    pcCardObj = _havePCCardParent.GetChild(i).gameObject;
                    break;
                }
            }

            if (pcCardObj == null)
            {
                pcCardObj = Instantiate(_pcCardPrefab, _havePCCardParent, false);
                _havePencilCaseCards.Add(pcCardObj);
            }

            pcCardObj.SetActive(true);

            return pcCardObj;
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

        /// <summary>
        /// 보유덱에 있는 모든 필통 카드 끄기
        /// </summary>
        public void AllFalseHavePCCard()
        {
            int count = _havePCCardParent.childCount;

            for (int i = 0; i < count; i++)
            {
                _havePCCardParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}