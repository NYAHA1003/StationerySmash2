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
        private CardPackSO cardPackSO; // �� ī���ѿ� ���� ���� 

        private CardPackInfo _pickCardPackInfo; // ���� ī���� ���� 

        [SerializeField]
        private List<int> _curCardAmountList = new List<int>(); // ���� ���� ���� ī��� ����Ʈ 
        [SerializeField]
        private List<CardNamingType> _curCardType = new List<CardNamingType>(); // ���� ���� ī��Ÿ�� ����Ʈ 

        [SerializeField]
        private List<CardData> DeckDataManagerSOHaveDeckDataList; // ������ �ִ� ī������ 
        [SerializeField]
        private List<CardNamingType> cardNamingTypes = new List<CardNamingType>(); // ������ �ִ� ī��Ÿ�� ����Ʈ
        [SerializeField]
        private List<CardNamingType> _notHaveCardNamingTypes = new List<CardNamingType>(); // ������ ���� ���� ī��Ÿ�� ����Ʈ


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
        private List<GachaCard> gachaCards = new List<GachaCard>(); // �� �����۰��� 
        [SerializeField]
        private GachaCard cardPrefab;

        private DeckSettingComponent _deckSettingComponent;
        private WarrningComponent _warningComponent; //���â
        private bool isNew = false;  // ���ο� ī�尡 ���� 

        /// <summary>
        /// �������� �ٲ�� ����
        /// </summary>
        [SerializeField]
        private int _quantity = 0;          //������ �����ȿ��� �������� �������� ���� 


        void Start()
        {
            //   UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindHaveCardData)
            _deckSettingComponent = FindObjectOfType<DeckSettingComponent>();
            ListenEvent();
            instantiateItem();
            //DeckDataManagerSOHaveDeckDataList = DeckDataManagerSO.HaveDeckDataList;
            UpdateCurrentCard();
        }

        #region ���ð��� 
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.DrawCardPack, (x) => ClickCardPack((int)x));
            EventManager.Instance.StartListening(EventsType.ActiveAndAnimateCard, () => ActiveAndAnimateCard());
            EventManager.Instance.StartListening(EventsType.CloseCardPack, () => CloseCardPackPanel());
        }
        /// <summary>
        /// �ִ�� ���� �̱� ������ ���� 
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
        /// ���� ī�� , ���� ī�� ������Ʈ 
        /// </summary>
        private void UpdateCurrentCard()
        {
            SetHaveCard();
            SetNotHaveCard();
        }
        /// <summary>
        /// ���� ī�尡 �������� ����
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
        /// ���� ī�尡 �������� ���� 
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
        /// ���� �ް�üũ 
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
        /// ������ �ʱ�ȭ 
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
        /// ī���� Ŭ���� 
        /// </summary>
        /// <param name="cardPackType"></param>
        private void ClickCardPack(int cardPackType)
        {
            //if (CheckCost(cardPackSO.cardPackInfos[cardPackType].useDalgona) == false)
            //{
            //    // panel.SetActive(true); 
            //    Debug.Log("�ް��� �����մϴ�. ");
            //    return;
            //}
            //         if(UserSaveManagerSO.UserSaveData._money < cardPackSO.cardPackInfos[cardPackType].useDalgona)
            //{
            //             _warningComponent ??= FindObjectOfType<WarrningComponent>();
            //             _warningComponent.SetWarrning("���� �����մϴ�");
            //             return;
            //}

            ResetData();
            DrawCardPack((PackageType)cardPackType);
            _gachaCanvas.SetActive(true);
            _cardPackPanel.SetActive(true);
            _cardMesh.StartMesh((PackageType)cardPackType);
        }
        /// <summary>
        /// ī���ѻ̱�� �����ͼ��� 
        /// </summary>
        /// <param name="cardPackType"></param>
        public void DrawCardPack(PackageType cardPackType)
        {
            int minCount, maxCount;

            // ī�� ���� ����
            _pickCardPackInfo = cardPackSO.cardPackInfos[(int)cardPackType];

            minCount = _pickCardPackInfo.minCount > cardNamingTypes.Count
                                ? cardNamingTypes.Count : _pickCardPackInfo.minCount;
            // ������ �ּҰ������� ī�尳���� �� ������ ī�� ������ 
            maxCount = _pickCardPackInfo.maxCount > cardNamingTypes.Count
                                ? cardNamingTypes.Count : _pickCardPackInfo.maxCount;

            _quantity = Random.Range(minCount, maxCount);

            // ī�� ����
            for (int i = 0; i < _quantity; i++)
            {
                int index = Random.Range(0, cardNamingTypes.Count);
                CardNamingType cardNamingType = cardNamingTypes[index];
                cardNamingTypes.Remove(cardNamingType);
                _curCardType.Add(cardNamingType);
            }
            // �� ī�� �̱� ����
            int newCardPercent = Random.Range(1, 101);
            isNew = _pickCardPackInfo.newCardPercent >= newCardPercent; // ���ο� �нǹ��� ��������

            SetCardSlices(_pickCardPackInfo.amount);
            SetGachaCard();

        }

        /// <summary>
        /// ���� ī���������� ������ �־�α� 
        /// </summary>
        private void SetCardSlices(int cardMaxAmount)
        {
            int temp = 0;
            int curQuantity = 0;
            int _currentUnitAmount = cardMaxAmount; // ���� �� �����ε� ó���� �� ī����� �ʱ�ȭ�� ���� ī�差��ŭ �� �� / ���� �� ���� �ʱ�ȭ 

            _curCardAmountList.Clear();
            //HaveUnit();
            //Shuffle();
            //QuantityOverCheck();

            int devide = cardMaxAmount / _quantity; // ���� �� ī�� / ���� ī�� ���� ����

            for (int i = 0; i < _quantity; i++)                                            //������ �������� ������ ������ŭ ����
            {
                if (i == _quantity - 1)                                                    //������ �ѿ��� ���� ī�尹����ŭ �־��ش�.
                {
                    _curCardAmountList.Add(_currentUnitAmount);
                    Debug.Log($"�п�ǰ ���� : {_curCardType[i]}, ���� : {_curCardAmountList[i]}");
                    return;
                }

                curQuantity = Random.Range(devide / 2 + temp, devide + temp);                   //�̹��� ���� ����
                _currentUnitAmount -= curQuantity;                                            //�����ִ� ���� ���� �������� ���� ������ ��                                         
                _curCardAmountList.Add(curQuantity);                                             //���ֺ� ���� �־��ֱ�
                temp = devide - curQuantity;                                                  //������ �߰��� ����
                Debug.Log($"�п�ǰ ���� : {_curCardType[i]}, ���� : {_curCardAmountList[i]}");
            }
        }



        /// <summary>
        /// ���� ī�� �� ���� ( ��������Ʈ, �̸� ��, ���������� ���� ) 
        /// </summary>
        private void SetGachaCard()
        {
            DeckCard curCard;
            for (int i = 0; i < _quantity; i++)
            {
                Debug.Log($"{i}��° ī�弳�� {_curCardType[i]} {_curCardAmountList[i]} ");
                UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_curCardType[i]), _curCardAmountList[i]); // ���������Ϳ� ī�������߰� 
                curCard = gachaCards[i].GetComponent<DeckCard>();
                curCard.SetCard(DeckDataManagerSO.FindHaveCardData(_curCardType[i]));
                gachaCards[i].SetSprite(curCard.CardImage.sprite, _backCardpack, true);
            }

            //������ ���� ���� ī�� �̱�
            if (isNew == true && _notHaveCardNamingTypes.Count > 0)
            {
                CardNamingType newCardNamingType = DrawNewCard();
                UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(newCardNamingType), 1); // ���������� ���� 
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
        /// �ű� ���� ȹ�� �Լ�
        /// </summary>
        private CardNamingType DrawNewCard()
        {
            CardNamingType _newCardNamingType;
            _newCardNamingType = _notHaveCardNamingTypes[Random.Range(0, _notHaveCardNamingTypes.Count)];     //���� ���ֵ��� ���ο� ������ ����
            // ������ ���� ���� ����Ʈ �ʱ�ȭ or NatHaveCardNamingTypes.Remove(cardNamingType); 
            Debug.Log($"���ο� ���� \"{_newCardNamingType}\"��/�� �������ϴ�.");
            return _newCardNamingType;
        } //���� ���߿� ���� �ϴ� �Լ���

    }


}



#region ���� �ڵ�
/*
 *         // �ӽ� ���� ������ ����
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
    //�ӽ� ���� ������ ���� ��

    /// <summary>
    /// �⺻���� �������ִ� ����
    /// </summary>
    int _min = 0;               //�ѿ��� ������ �ּ� ����
    int _max = 0;               //�ѿ��� ������ �ִ� ����
    int _newPercent = 0;        //�ű� ĳ���Ͱ� ���� Ȯ��(�Ѵ�)
    int _useMoney = 0;          //����ϴ� �� ����
    int _useDalgona = 0;        //����ϴ� �ް� ����
    int _unitMaxAmount = 0; // ���� �� ī�� 
    int _newChPercent = 0;      //���� ���� �ۼ�Ʈ 
    int _newCharacter = 0;      //���ο� �нǹ��� ���� ���� ��ȣ

    private List<string> _UnitNameList = new List<string>();
    private List<int> _NHnum = new List<int>();

 *         /// <summary>
/// ������ �ִ°Ͱ� ������ ���� ���� �п�ǰ�� �ҷ����� �Լ�
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

    _UnitNameList.Add("����");                 //Pencil
    _UnitNameList.Add("����");                 //MechPencil
    _UnitNameList.Add("���찳");               //Eraser
    _UnitNameList.Add("����");                 //Scissors
    _UnitNameList.Add("Ǯ");                   //Glue
    _UnitNameList.Add("��");                   //Ruler
    _UnitNameList.Add("Ŀ��Į");               //Cutterknife
    _UnitNameList.Add("����Ʈ��");             //Postit
    _UnitNameList.Add("������");               //MechaPencilLead
    _UnitNameList.Add("����");                 //Pen
}
/// <summary>
/// ������ ������ ����ִ� �Լ�
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
/// ������ �ִ� ���� �迭�� �������� 
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
/// ������ ���� ���� ���� �迭�� �������� 
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
/// ������ ������ �ִ� ĳ������ �������� ũ�ٸ� �ִ��� ĳ������ ������ ���� 
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
/// ��Ű���� ������� �������� ���� ����
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
///// Common��Ű���� �����Ҷ� �����ϴ� �Լ�
///// </summary>
//private void CommonButtonClick()
//{
//    SetPakageSelect(PackageType.CommonPack);
//    /*if(���� ���� �� < _useMoney)
//    {
//        return;
//    }*/
//    RandomUnitSummons();
//    RandomNewUnit();
//}

///// <summary>
///// Shiny��Ű���� �����Ҷ� �����ϴ� �Լ�
///// </summary>
//private void ShinyButtonClick()
//{
//    SetPakageSelect(PackageType.ShinyPack);
//    /*if(���� ���� �ް� <  _useDalgona)
//    {
//        return;
//    }*/
//    //���� �ް� -= _useDalgona;
//    RandomUnitSummons();
//    RandomNewUnit();
//}

///// <summary>
///// Legendary��Ű���� �����Ҷ� �����ϴ� �Լ�
///// </summary>
//private void LegendaryButtonClick()
//{
//    SetPakageSelect(PackageType.LegendaryPack);
//    /*if(���� ���� �ް� <  _useDalgona)
//    {
//        return;
//    }*/
//    RandomUnitSummons();
//    RandomNewUnit();
//}
