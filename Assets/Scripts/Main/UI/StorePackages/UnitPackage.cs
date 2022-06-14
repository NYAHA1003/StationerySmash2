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

namespace Main.Store
{
    public class UnitPackage : MonoBehaviour
    {        
        // �ӽ� ���� ������ ����
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

        //-----------------------------------------------------------------------------------//

        [SerializeField]
        private CardPackSO cardPackSO; // �� ī���ѿ� ���� ���� 

        private CardPackInfo _pickCardPackInfo; // ���� ī���� ���� 

        int _currentUnitAmount = 0; // ���� �� �����ε� ó���� �� ī����� �ʱ�ȭ�� ���� ī�差��ŭ �� �� / ���� �� ���� �ʱ�ȭ 

        [SerializeField]
        private List<int> _curCardAmountList = new List<int>(); // ���� ���� ���� ī��� ����Ʈ 
        [SerializeField]
        private List<CardNamingType> _curCardType = new List<CardNamingType>(); // ���� ���� ī��Ÿ�� ����Ʈ 

        [SerializeField]
        private List<CardData> haveCardList = DeckDataManagerSO.HaveDeckDataList; // ������ �ִ� ī������ 
        [SerializeField]
        private List<CardNamingType> cardNamingTypes = new List<CardNamingType>(); // ������ �ִ� ī��Ÿ�� ����Ʈ
        [SerializeField]
        private List<CardNamingType> NatHaveCardNamingTypes = new List<CardNamingType>(); // ������ ���� ���� ī��Ÿ�� ����Ʈ

        [SerializeField]
        private GachaInfo _gachaInfo;

        [SerializeField]
        private List<GachaCard> gachaCards = new List<GachaCard>(); // �� �����۰��� 
        [SerializeField]
        private GachaCard cardPrefab; 

        /// <summary>
        /// �������� �ٲ�� ����
        /// </summary>
        int _quantity = 0;          //������ �����ȿ��� �������� �������� ���� 


        void Start()
        {
            ListenEvent();
            instantiateItem(); 
            UpdateCurrentCard();
        }

        #region ���ð��� 
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.DrawCardPack, (x) => ClickCardPack((int)x));
        }
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
            int haveCardCount = haveCardList.Count;

            for (int i = 0; i < haveCardCount; i++)
            {
                cardNamingTypes.Add(haveCardList[i]._cardNamingType);
            }
        }
        /// <summary>
        /// ���� ī�尡 �������� ���� 
        /// </summary>
        private void SetNotHaveCard()
        {
            NatHaveCardNamingTypes.Clear();

            int cardCount = System.Enum.GetValues(typeof(CardNamingType)).Length;
            bool isNotHave = true;
            for (int i = 0; i < cardCount; i++)
            {
                isNotHave = true;
                cardNamingTypes.ForEach((x) =>
                {
                    if (x == (CardNamingType)i)
                    {
                        isNotHave = false;
                    }
                });

                if (isNotHave == true)
                {
                    NatHaveCardNamingTypes.Add((CardNamingType)i);
                }
            }

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
        private void Reset()
        {
            _curCardAmountList.Clear();
            _curCardType.Clear(); 
            _quantity = 0;
            _currentUnitAmount = 0;
        }

        /// <summary>
        /// ī���� Ŭ���� 
        /// </summary>
        /// <param name="cardPackType"></param>
        private void ClickCardPack(int cardPackType)
        {
            if (CheckCost(cardPackSO.cardPackInfos[cardPackType].useDalgona) == false)
            {
                // panel.SetActive(true); 
                Debug.Log("�ް��� �����մϴ�. ");
                return;
            }
            Reset(); 
            DrawCardPack((PackageType)cardPackType);
        }

        /// <summary>
        /// ���� ī�� �� ���� ( ��������Ʈ, �̸� ��) 
        /// </summary>
        private void SetGachaCard(bool isNew)
        {
            for (int i = 0; i < _quantity; i++)
            {
                gachaCards[i].GetComponent<DeckCard>().SetCard(DeckDataManagerSO.FindHaveCardData(_curCardType[i]));
                gachaCards[i].ActiveAndAnimate();
            }
           if(isNew == true)
            {
                CardNamingType newCardNamingType =  DrawNewCard();
                gachaCards[_quantity],
            }
        }
        /// <summary>
        /// �ִ�� ���� �̱� ������ ���� 
        /// </summary>
        private void instantiateItem()
        {
            for(int i =0; i < _gachaInfo.gachaSO.maxAmount; i++)
            {
                GachaCard gachaCard = Instantiate(cardPrefab, _gachaInfo.itemsParent.transform);
                gachaCard.gameObject.SetActive(false); 
                gachaCards.Add(gachaCard); 
            }

        }
        /// <summary>
        /// ī���ѻ̱�� �����ͼ��� 
        /// </summary>
        /// <param name="cardPackType"></param>
        public void DrawCardPack(PackageType cardPackType)
        {
            int minCount, maxCount;

            // ī�� ����
            _pickCardPackInfo = cardPackSO.cardPackInfos[(int)cardPackType];

            minCount = _pickCardPackInfo.minCount > cardNamingTypes.Count
                                ? cardNamingTypes.Count : _pickCardPackInfo.minCount;
            // ������ �ּҰ������� ī�尳���� �� ������ ī�� ������ 
            maxCount = _pickCardPackInfo.maxCount > cardNamingTypes.Count
                                ? cardNamingTypes.Count : _pickCardPackInfo.maxCount;

            _quantity = Random.Range(minCount, maxCount);

            // ī�� ����
            bool isOverlap = false;
            for (int i = 0; i < _quantity; i++)
            {
                int index = Random.Range(0, cardNamingTypes.Count);
                CardNamingType cardNamingType = cardNamingTypes[index];
                _curCardType.ForEach((x) =>
                {
                    if (x == cardNamingType)
                    {
                        isOverlap = true;
                    }
                });
                if (isOverlap == true)
                {
                    --i;
                    continue;
                }
                _curCardType.Add(cardNamingType);
            }
            // �� ī�� �̱� ����
            int newCardPercent = Random.Range(1, 101);
            bool isNewCard = _pickCardPackInfo.newCardPercent <= newCardPercent; // ���ο� �нǹ��� ��������

            SetCardSlices(_quantity, _pickCardPackInfo.amount);
            SetGachaCard(isNewCard); 
            //if (isNewCard == true)
            //{
            //    DrawNewCard();
            //}
        }

        /// <summary>
        /// ���� ī���������� ������ �־�α� 
        /// </summary>
        private void SetCardSlices(int quantity, int cardMaxAmount)
        {
            int temp = 0;
            int curQuantity = 0;

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
                    Debug.Log($"�п�ǰ ���� : {_UnitNameList[_NHnum[i]]}, ���� : {_curCardAmountList[i]}");
                    return;
                }

                curQuantity = Random.Range(devide / 2 + temp, devide + temp);                   //�̹��� ���� ����
                _currentUnitAmount -= curQuantity;                                            //�����ִ� ���� ���� �������� ���� ������ ��                                         
                _curCardAmountList.Add(curQuantity);                                             //���ֺ� ���� �־��ֱ�
                temp = devide - curQuantity;                                                  //������ �߰��� ����
                Debug.Log($"�п�ǰ ���� : {_UnitNameList[_NHnum[i]]}, ���� : {_curCardAmountList[i]}");
            }
        }

        /// <summary>
        /// �ű� ���� ȹ�� �Լ�
        /// </summary>
        private CardNamingType DrawNewCard()
        {
            CardNamingType _newCardNamingType;
            _newCardNamingType = NatHaveCardNamingTypes[Random.Range(0, NatHaveCardNamingTypes.Count)];     //���� ���ֵ��� ���ο� ������ ����
            UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(_newCardNamingType)); // ���������� ���� 
            // ������ ���� ���� ����Ʈ �ʱ�ȭ or NatHaveCardNamingTypes.Remove(cardNamingType); 
            Debug.Log($"���ο� ���� \"{_newCardNamingType}\"��/�� �������ϴ�.");
            return _newCardNamingType; 
        } //���� ���߿� ���� �ϴ� �Լ���
    }

    #region ���� �ڵ�
    /*
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
}




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
