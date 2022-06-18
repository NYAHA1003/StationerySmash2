using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Utill.Data;
using Utill.Tool;
using System.Threading.Tasks;
using UnityEngine.UI;
using Main.Event;
using Main.Deck;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace Main.Store
{
    public class UnitPackage : MonoBehaviour
    {
        [SerializeField]
        private CardPackSO cardPackSO; // 각 카드팩에 대한 정보 

        private CardPackInfo _pickCardPackInfo; // 뽑은 카드팩 정보 

        [SerializeField]
        private List<int> _curCardAmountList = new List<int>(); // 현재 나눈 뽑을 카드양 리스트 
        [SerializeField]
        private List<CardNamingType> _curCardType = new List<CardNamingType>(); // 현재 나올 카드타입 리스트 

        [SerializeField]
        private List<CardData> DeckDataManagerSOHaveDeckDataList; // 가지고 있는 카드정보 
        [SerializeField]
        private List<CardNamingType> cardNamingTypes = new List<CardNamingType>(); // 가지고 있는 카드타입 리스트
        [SerializeField]
        private List<CardNamingType> _notHaveCardNamingTypes = new List<CardNamingType>(); // 가지고 있지 않은 카드타입 리스트


        [SerializeField]
        private GachaInfo _gachaInfo;
        [SerializeField]
        private Sprite _backCardpack;
        [SerializeField]
        private GameObject _cardPackPanel;
        [SerializeField]
        private GameObject _gachaCanvas;
        [SerializeField]
        private CardMesh _cardMesh;

        [SerializeField]
        private List<GachaCard> gachaCards = new List<GachaCard>(); // 총 아이템개수 
        [SerializeField]
        private GachaCard cardPrefab;

        private DeckSettingComponent _deckSettingComponent;
        private WarrningComponent _warningComponent; //경고창
        private bool isNew = false;  // 새로운 카드가 떴냐 

        /// <summary>
        /// 랜덤으로 바뀌는 정보
        /// </summary>
        [SerializeField]
        private int _quantity = 0;          //정해진 범위안에서 랜덤으로 정해지는 수량 


        void Start()
        {
            //   UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindHaveCardData)
            _deckSettingComponent = FindObjectOfType<DeckSettingComponent>();
            ListenEvent();
            instantiateItem();
            //DeckDataManagerSOHaveDeckDataList = DeckDataManagerSO.HaveDeckDataList;
            UpdateCurrentCard();
        }

        #region 세팅관련 
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.DrawCardPack, (x) => ClickCardPack((int)x));
            EventManager.Instance.StartListening(EventsType.ActiveAndAnimateCard, () => ActiveAndAnimateCard());
            EventManager.Instance.StartListening(EventsType.CloseCardPack, () => CloseCardPackPanel());
        }
        /// <summary>
        /// 최대로 나올 뽑기 아이템 생성 
        /// </summary>
        private void instantiateItem()
        {
            for (int i = 0; i < _gachaInfo.gachaSO.maxAmount; i++)
            {
                GachaCard gachaCard = Instantiate(cardPrefab, _gachaInfo.itemsParent.transform);
                gachaCard.gameObject.SetActive(false);
                gachaCards.Add(gachaCard);
            }
        }

        /// <summary>
        /// 보유 카드 , 비보유 카드 업데이트 
        /// </summary>
        private void UpdateCurrentCard()
        {
            SetHaveCard();
            SetNotHaveCard();
        }
        /// <summary>
        /// 보유 카드가 무엇인지 설정
        /// </summary>
        private void SetHaveCard()
        {
            cardNamingTypes.Clear();
            int haveCardCount = DeckDataManagerSO.HaveDeckDataList.Count;

            for (int i = 0; i < haveCardCount; i++)
            {
                cardNamingTypes.Add(DeckDataManagerSO.HaveDeckDataList[i]._cardNamingType);
            }
        }
        /// <summary>
        /// 비보유 카드가 무엇인지 설정 
        /// </summary>
        private void SetNotHaveCard()
        {
            _notHaveCardNamingTypes.Clear();

            List<CardData> allCardList = DeckDataManagerSO.StdDeckDataList.ToList();

            foreach (var type in System.Enum.GetValues(typeof(CardNamingType)))
            {
                if ((int)type > 1000)
                {
                    allCardList.Remove(DeckDataManagerSO.FindStdCardData((CardNamingType)type));
                }
            }

            cardNamingTypes.ForEach((x) =>
            {
                allCardList.Remove(DeckDataManagerSO.FindStdCardData(x));
            });

            allCardList.ForEach((x) =>
            {
                _notHaveCardNamingTypes.Add(x._cardNamingType);
            });

            //int cardCount = System.Enum.GetValues(typeof(CardNamingType)).Length;
            //bool isNotHave = true;
            //for (int i = 100; i < cardCount; i+=100)
            //{
            //    isNotHave = true;
            //    cardNamingTypes.ForEach((x) =>
            //    {
            //        if (x == (CardNamingType)i)
            //        {
            //            isNotHave = false;
            //        }
            //    });

            //    if (isNotHave == true)
            //    {
            //        _notHaveCardNamingTypes.Add((CardNamingType)i);
            //    }
            //}

        }

        #endregion


        /// <summary>
        /// 보유 달고나체크 
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        private bool CheckCost(int cost)
        {
            if (UserSaveManagerSO.UserSaveData._dalgona > cost)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 데이터 초기화 
        /// </summary>
        private void ResetData()
        {
            for (int i = 0; i < _gachaInfo.gachaSO.maxAmount; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
            }
            _curCardAmountList.Clear();
            _curCardType.Clear();
            _quantity = 0;
            UpdateCurrentCard();
        }
        private void CloseCardPackPanel()
        {
            _gachaCanvas.SetActive(false);
            _cardPackPanel.SetActive(false);
        }
        /// <summary>
        /// 카드팩 클릭시 
        /// </summary>
        /// <param name="cardPackType"></param>
        private void ClickCardPack(int cardPackType)
        {
            //if (CheckCost(cardPackSO.cardPackInfos[cardPackType].useDalgona) == false)
            //{
            //    // panel.SetActive(true); 
            //    Debug.Log("달고나가 부족합니다. ");
            //    return;
            //}
            //         if(UserSaveManagerSO.UserSaveData._money < cardPackSO.cardPackInfos[cardPackType].useDalgona)
            //{
            //             _warningComponent ??= FindObjectOfType<WarrningComponent>();
            //             _warningComponent.SetWarrning("돈이 부족합니다");
            //             return;
            //}

            ResetData();
            DrawCardPack((PackageType)cardPackType);
            _gachaCanvas.SetActive(true);
            _cardPackPanel.SetActive(true);
            _cardMesh.StartMesh((PackageType)cardPackType);
        }
        /// <summary>
        /// 카드팩뽑기시 데이터세팅 
        /// </summary>
        /// <param name="cardPackType"></param>
        public void DrawCardPack(PackageType cardPackType)
        {
            int minCount, maxCount;

            // 카드 종류 개수
            _pickCardPackInfo = cardPackSO.cardPackInfos[(int)cardPackType];

            minCount = _pickCardPackInfo.minCount > cardNamingTypes.Count
                                ? cardNamingTypes.Count : _pickCardPackInfo.minCount;
            // 설정할 최소개수보다 카드개수가 더 적으면 카드 개수로 
            maxCount = _pickCardPackInfo.maxCount > cardNamingTypes.Count
                                ? cardNamingTypes.Count : _pickCardPackInfo.maxCount;

            _quantity = Random.Range(minCount, maxCount);

            // 카드 종류
            for (int i = 0; i < _quantity; i++)
            {
                int index = Random.Range(0, cardNamingTypes.Count);
                CardNamingType cardNamingType = cardNamingTypes[index];
                cardNamingTypes.Remove(cardNamingType);
                _curCardType.Add(cardNamingType);
            }
            // 새 카드 뽑기 여부
            int newCardPercent = Random.Range(1, 101);
            isNew = _pickCardPackInfo.newCardPercent >= newCardPercent; // 새로운 분실물이 나오는지

            SetCardSlices(_pickCardPackInfo.amount);
            SetGachaCard();

        }

        /// <summary>
        /// 랜덤 카드조각개수 나눠서 넣어두기 
        /// </summary>
        private void SetCardSlices(int cardMaxAmount)
        {
            int temp = 0;
            int curQuantity = 0;
            int _currentUnitAmount = cardMaxAmount; // 나올 총 유닛인데 처음엔 총 카드수로 초기화후 뽑은 카드량만큼 뺄 것 / 뽑을 때 마다 초기화 

            _curCardAmountList.Clear();
            //HaveUnit();
            //Shuffle();
            //QuantityOverCheck();

            int devide = cardMaxAmount / _quantity; // 나올 총 카드 / 나올 카드 종류 개수

            for (int i = 0; i < _quantity; i++)                                            //위에서 랜덤으로 정해진 수량만큼 실행
            {
                if (i == _quantity - 1)                                                    //마지막 팩에는 남은 카드갯수만큼 넣어준다.
                {
                    _curCardAmountList.Add(_currentUnitAmount);
                    Debug.Log($"학용품 종류 : {_curCardType[i]}, 수량 : {_curCardAmountList[i]}");
                    return;
                }

                curQuantity = Random.Range(devide / 2 + temp, devide + temp);                   //이번에 나올 수량
                _currentUnitAmount -= curQuantity;                                            //남아있는 뽑힐 유닛 갯수에서 나온 수량을 뺌                                         
                _curCardAmountList.Add(curQuantity);                                             //유닛별 수량 넣어주기
                temp = devide - curQuantity;                                                  //다음에 추가될 수량
                Debug.Log($"학용품 종류 : {_curCardType[i]}, 수량 : {_curCardAmountList[i]}");
            }
        }



        /// <summary>
        /// 뽑은 카드 값 설정 ( 스프라이트, 이름 등, 유저데이터 설정 ) 
        /// </summary>
        private void SetGachaCard()
        {
            DeckCard curCard;
            for (int i = 0; i < _quantity; i++)
            {
                Debug.Log($"{i}번째 카드설정 {_curCardType[i]} {_curCardAmountList[i]} ");
                UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_curCardType[i]), _curCardAmountList[i]); // 유저데이터에 카드조각추가 
                curCard = gachaCards[i].GetComponent<DeckCard>();
                curCard.SetCard(DeckDataManagerSO.FindHaveCardData(_curCardType[i]));
                gachaCards[i].SetSprite(curCard.CardImage.sprite, _backCardpack, true);
            }

            //가지고 있지 않은 카드 뽑기
            if (isNew == true && _notHaveCardNamingTypes.Count > 0)
            {
                CardNamingType newCardNamingType = DrawNewCard();
                UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(newCardNamingType), 1); // 유저데이터 저장 
                curCard = gachaCards[_quantity].GetComponent<DeckCard>();
                curCard.SetCard(DeckDataManagerSO.FindHaveCardData(newCardNamingType));
                gachaCards[_quantity].SetSprite(curCard.CardImage.sprite, _backCardpack, true);
            }
            _deckSettingComponent.UpdateDeck();
            _deckSettingComponent.UpdateHaveAndEquipDeck();
        }

        private void ActiveAndAnimateCard()
        {
            int count = (isNew && _notHaveCardNamingTypes.Count > 0) ? _quantity : _quantity - 1;
            for (int i = 0; i <= count; i++)
            {
                gachaCards[i].ActiveAndAnimate();
            }
        }

        /// <summary>
        /// 신규 유닛 획득 함수
        /// </summary>
        private CardNamingType DrawNewCard()
        {
            CardNamingType _newCardNamingType;
            _newCardNamingType = _notHaveCardNamingTypes[Random.Range(0, _notHaveCardNamingTypes.Count)];     //없는 유닛들중 새로운 유닛을 선택
            // 가지고 있지 않은 리스트 초기화 or NatHaveCardNamingTypes.Remove(cardNamingType); 
            Debug.Log($"새로운 유닛 \"{_newCardNamingType}\"이/가 뽑혔습니다.");
            return _newCardNamingType;
        } //제일 나중에 들어가야 하는 함수임

    }


}



#region 이전 코드
/*
 *         // 임시 보유 데이터 선언
    private List<bool> _IsHave = new List<bool>();
    private bool _Pencil = true;
    private bool _MechaPencil = true;
    private bool _Eraser = true;
    private bool _Scissors = false;
    private bool _Glue = false;
    private bool _Ruler = false;
    private bool _Cutterknife = false;
    private bool _Postit = false;
    private bool _MechaPencilLead = false;
    private bool _Pen = false;
    //임시 보유 데이터 선언 끝

    /// <summary>
    /// 기본으로 정해져있는 정보
    /// </summary>
    int _min = 0;               //팩에서 나오는 최소 갯수
    int _max = 0;               //팩에서 나오는 최대 갯수
    int _newPercent = 0;        //신규 캐릭터가 나올 확률(팩당)
    int _useMoney = 0;          //사용하는 돈 수량
    int _useDalgona = 0;        //사용하는 달고나 갯수
    int _unitMaxAmount = 0; // 나올 총 카드 
    int _newChPercent = 0;      //랜덤 숫자 퍼센트 
    int _newCharacter = 0;      //새로운 분실물로 뽑힌 유닛 번호

    private List<string> _UnitNameList = new List<string>();
    private List<int> _NHnum = new List<int>();

 *         /// <summary>
/// 가지고 있는것과 가지고 있지 않은 학용품을 불러오는 함수
/// </summary>
private void SetList()
{
    _IsHave.Add(_Pencil);
    _IsHave.Add(_MechaPencil);
    _IsHave.Add(_Eraser);
    _IsHave.Add(_Scissors);
    _IsHave.Add(_Glue);
    _IsHave.Add(_Ruler);
    _IsHave.Add(_Cutterknife);
    _IsHave.Add(_Postit);
    _IsHave.Add(_MechaPencilLead);
    _IsHave.Add(_Pen);

    _UnitNameList.Add("연필");                 //Pencil
    _UnitNameList.Add("샤프");                 //MechPencil
    _UnitNameList.Add("지우개");               //Eraser
    _UnitNameList.Add("가위");                 //Scissors
    _UnitNameList.Add("풀");                   //Glue
    _UnitNameList.Add("자");                   //Ruler
    _UnitNameList.Add("커터칼");               //Cutterknife
    _UnitNameList.Add("포스트잇");             //Postit
    _UnitNameList.Add("샤프심");               //MechaPencilLead
    _UnitNameList.Add("볼펜");                 //Pen
}
/// <summary>
/// 랜덤한 유닛을 골라주는 함수
/// </summary>
private void Shuffle()
{
    int temp = 0;
    for (int i = 0; i < 100; i++)
    {
        int change = Random.Range(0, _NHnum.Count);
        int change1 = Random.Range(0, _NHnum.Count);
        temp = _NHnum[change];
        _NHnum[change] = _NHnum[change1];
        _NHnum[change1] = temp;
    }
}

/// <summary>
/// 가지고 있는 유닛 배열로 가져오기 
/// </summary>
private void HaveUnit()
{
    int j = -1;

    _NHnum.Clear();

    for (int i = 0; i < _IsHave.Count; i++)
    {
        if (_IsHave[i])
        {
            j++;
            _NHnum.Add(i);
        }
    }
}

/// <summary>
/// 가지고 있지 않은 유닛 배열로 가져오기 
/// </summary>
private void NotHaveUnit()
{
    int j = -1;

    _NHnum.Clear();

    for (int i = 0; i < _IsHave.Count; i++)
    {
        if (!_IsHave[i])
        {
            j++;
            _NHnum.Add(i);
        }
    }
}

/// <summary>
/// 수량이 가지고 있는 캐릭터의 갯수보다 크다면 최댓값을 캐릭터의 갯수로 고정 
/// </summary>
private void QuantityOverCheck()
{
    if (_Quantity > _NHnum.Count)
    {
        _Quantity = _NHnum.Count;
    }
}
}

/// <summary>
/// 패키지를 골랐을때 여러가지 값을 대입
/// </summary>
/// <param name="cardPackType"></param>
public void SetPakageSelect(PackageType cardPackType)
{
//switch (cardPackType)
//{
//    case PackageType.CommonPack:
//        _min = 2; _max = 3;
//        _newPercent = 1;
//        _useMoney = 500;
//        _unitMaxAmount = 20;
//        _CurrentUnitAmount = _unitMaxAmount;
//        break;
//    case PackageType.ShinyPack:
//        _min = 4; _max = 5;
//        _newPercent = 4;
//        _useDalgona = 10;
//        _unitMaxAmount = 68;
//        _CurrentUnitAmount = _unitMaxAmount;
//        break;
//    case PackageType.LegendaryPack:
//        _min = 6; _max = 8;
//        _newPercent = 15;
//        _useDalgona = 45;
//        _unitMaxAmount = 300;
//        _CurrentUnitAmount = _unitMaxAmount;
//        break;
//    default:
//        break;
//}
_Quantity = Random.Range(_min, _max + 1);
_newChPercent = Random.Range(0, 100 + 1);
}
*/
#endregion
///// <summary>
///// Common패키지를 구매할때 실행하는 함수
///// </summary>
//private void CommonButtonClick()
//{
//    SetPakageSelect(PackageType.CommonPack);
//    /*if(현재 가진 돈 < _useMoney)
//    {
//        return;
//    }*/
//    RandomUnitSummons();
//    RandomNewUnit();
//}

///// <summary>
///// Shiny패키지를 구매할때 실행하는 함수
///// </summary>
//private void ShinyButtonClick()
//{
//    SetPakageSelect(PackageType.ShinyPack);
//    /*if(현재 가진 달고나 <  _useDalgona)
//    {
//        return;
//    }*/
//    //현재 달고나 -= _useDalgona;
//    RandomUnitSummons();
//    RandomNewUnit();
//}

///// <summary>
///// Legendary패키지를 구매할때 실행하는 함수
///// </summary>
//private void LegendaryButtonClick()
//{
//    SetPakageSelect(PackageType.LegendaryPack);
//    /*if(현재 가진 달고나 <  _useDalgona)
//    {
//        return;
//    }*/
//    RandomUnitSummons();
//    RandomNewUnit();
//}
