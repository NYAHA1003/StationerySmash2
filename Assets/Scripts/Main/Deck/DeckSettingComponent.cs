using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Event;

namespace Main.Deck
{
    public class DeckSettingComponent : MonoBehaviour, IUserData
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
        private Button _presetButton1 = null;
        [SerializeField]
        private Button _presetButton2 = null;
        [SerializeField]
        private Button _presetButton3 = null;

        private bool _isActivePC; //필통 스크롤이 켜져있는지

        private void Awake()
        {
            SaveManager._instance.SaveData.AddObserver(this);
        }

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
            EventManager.StartListening(EventsType.ChangePCAndDeck, OnChangePencilAndCards);
            EventManager.StartListening(EventsType.UpdateHaveAndEquipDeck, UpdateHaveAndEquipDeck);
            EventManager.StartListening(EventsType.UpdateHaveAndEquipPCDeck, UpdateHaveAndEquipPCDeck);
        }

        /// <summary>
        /// 정렬 방식을 바꾼다
        /// </summary>
        public void ChangeSortSystem(int type)
		{
            switch(type)
			{
                //가나다
                case 0:
                    _userDeckData.HaveDeckSortABC();
                    break;

                //코스트
                case 1:
                    _userDeckData.HaveDeckSortCost();
                    break;
            }
            UpdateHaveAndEquipDeck();
        }
        

        /// <summary>
        /// 보유 덱과 장착 덱을 새로고침한다
        /// </summary>
        public void UpdateHaveAndEquipDeck()
        {
            AllFalseHaveCard();
            SetHaveDeck();
            AllFalseEquipCard();
            SetEquipDeck();
        }

        /// <summary>
        /// 카드 덱과 필통 덱을 전환한다
        /// </summary>
        public void OnChangePencilAndCards()
        {
            UpdateHaveAndEquipDeck();
            UpdateHaveAndEquipPCDeck();

            _isActivePC = !_isActivePC;

            _haveDeckScroll.SetActive(!_isActivePC);
            _equipDeckScroll.SetActive(!_isActivePC);

            _havePCDeckScroll.SetActive(_isActivePC);
            _equipPCDeckScroll.SetActive(_isActivePC);

        }
        public void Notify(ref UserSaveData userSaveData)
        {
            UpdateHaveAndEquipDeck();
            UpdateHaveAndEquipPCDeck();
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
            _userDeckData.ChangePreset(index);
            _userDeckData.SetCardData();
            _userDeckData.SetPencilCaseData();
            UpdateHaveAndEquipDeck();
            UpdateHaveAndEquipPCDeck();
        }

        /// <summary>
        /// 보유 덱에 카드 세팅 
        /// </summary>
        public void SetHaveDeck()
        {
            for (int i = 0; i < _userDeckData._haveDeckListSO.cardDatas.Count; i++)
            {
                GameObject cardObj = PoolHaveCard();
                Button cardButton = cardObj.GetComponent<Button>();
                cardObj.GetComponent<DeckCard>().SetCard(_userDeckData._haveDeckListSO.cardDatas[i]);
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
            for (int i = 0; i < _userDeckData._inGameDeckListSO.cardDatas.Count; i++)
			{
				GameObject cardObj = PoolEquipCard();
				Button cardButton = cardObj.GetComponent<Button>();
				cardObj.GetComponent<DeckCard>().SetCard(_userDeckData._inGameDeckListSO.cardDatas[i]);
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
            _userDeckData.SetPencilCaseData();
            for (int i = 0; i < _userDeckData._havePCDataSO._pencilCaseDataList.Count; i++)
            {
                GameObject cardObj = PoolHavePCCard();
                Button cardButton = cardObj.GetComponent<Button>();
                PencilCaseData pencilCaseData = _userDeckData._havePCDataSO._pencilCaseDataList[i];
                cardObj.GetComponent<PencilCaseCard>().SetPencilCaseData(pencilCaseData);
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() =>
                {
                   EventManager.TriggerEvent(EventsType.ActivePencilCaseDescription, pencilCaseData);
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
                _haveDeckCards[i].GetComponent<DeckCard>().SetCard(_userDeckData._haveDeckListSO.cardDatas[i]);
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