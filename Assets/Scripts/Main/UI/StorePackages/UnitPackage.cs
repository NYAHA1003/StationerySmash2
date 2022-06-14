using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Utill.Data;
using Utill.Tool;
using System.Threading.Tasks;
using UnityEngine.UI;
using Main.Event;

namespace Main.Store
{
    public class UnitPackage : MonoBehaviour
    {
        //�ӽ� ���� ������ ���� ����
        private List<string> _UnitNameList = new List<string>();
        private List<bool> _IsHave = new List<bool>();
        private List<int> _NHnum = new List<int>();
        private List<int> _curCardAmountList = new List<int>();

       
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

        [SerializeField]
        private CardPackSO cardPackSO; // �� ī���ѿ� ���� ���� 

        private CardPackInfo _pickCardPackInfo; // ���� ī���� ���� 

        int _CurrentUnitAmount = 0; // ���� �� �����ε� ó���� �� ī����� �ʱ�ȭ�� ���� ī�差��ŭ �� �� / ���� �� ���� �ʱ�ȭ 
        int _unitMaxAmount = 0; // ���� �� ī�� 


        /// <summary>
        /// �������� �ٲ�� ����
        /// </summary>
        int _Quantity = 0;          //������ �����ȿ��� �������� �������� ���� 
        int _newChPercent = 0;      //���� ���� �ۼ�Ʈ 
        int _newCharacter = 0;      //���ο� �нǹ��� ���� ���� ��ȣ

        void Start()
        {
            SetList();
        }

        private void ListenEvent()
        {
            //EventManager.Instance.StartListening(EventsType.)
        }
        /// <summary>
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

        private void CardPackClick(int cardPackType)
        {
            SetPakageSelect((PackageType)cardPackType);
            /*if(���� ���� �� < _useMoney)
            {
                return;
            }*/
            // RandomUnitSummons();
            RandomNewUnit();
        }
        
        public void DrawCardPack(PackageType cardPackType)
        {
            _pickCardPackInfo = cardPackSO.cardPackInfos[(int)cardPackType];

            int quantity = Random.Range(_pickCardPackInfo.minCount, _pickCardPackInfo.maxCount);
            int newCardPercent = Random.Range(1, 101); 

            bool isNewCard = _pickCardPackInfo.newCardPercent <= newCardPercent; // ���ο� �нǹ��� ��������

            RandomUnitSummons(quantity, _pickCardPackInfo.amount);
        }
        /// <summary>
        /// ��Ű���� ������� �������� ���� ����
        /// </summary>
        /// <param name="cardPackType"></param>
        public void SetPakageSelect(PackageType cardPackType)
        {
            switch (cardPackType)
            {
                case PackageType.CommonPack:
                    _min = 2; _max = 3;
                    _newPercent = 1;
                    _useMoney = 500;
                    _unitMaxAmount = 20;
                    _CurrentUnitAmount = _unitMaxAmount;
                    break;
                case PackageType.ShinyPack:
                    _min = 4; _max = 5;
                    _newPercent = 4;
                    _useDalgona = 10;
                    _unitMaxAmount = 68;
                    _CurrentUnitAmount = _unitMaxAmount;
                    break;
                case PackageType.LegendaryPack:
                    _min = 6; _max = 8;
                    _newPercent = 15;
                    _useDalgona = 45;
                    _unitMaxAmount = 300;
                    _CurrentUnitAmount = _unitMaxAmount; 
                    break;
                default:
                    break;
            }
            _Quantity = Random.Range(_min, _max + 1);
            _newChPercent = Random.Range(0, 100 + 1);
        }

        /// <summary>
        /// ���� ī���������� ������ �־�α� 
        /// </summary>
        private void RandomUnitSummons(int quantity, int cardMaxAmount)
        {
            int temp = 0;
            int curQuantity = 0;

            _curCardAmountList.Clear();
            //HaveUnit();
            //Shuffle();
            //QuantityOverCheck();


            int devide = cardMaxAmount / _Quantity; // ���� �� ī�� / ���� ī�� ���� 

            for (int i = 0; i < _Quantity; i++)                                            //������ �������� ������ ������ŭ ����
            {
                if (i == _Quantity - 1)                                                    //������ �ѿ��� ���� ī�尹����ŭ �־��ش�.
                {
                    _curCardAmountList.Add(_CurrentUnitAmount);
                    Debug.Log($"�п�ǰ ���� : {_UnitNameList[_NHnum[i]]}, ���� : {_curCardAmountList[i]}");
                    return;
                }

                curQuantity = Random.Range(devide/2 + temp, devide + temp);                   //�̹��� ���� ����
                _CurrentUnitAmount -= curQuantity;                                            //�����ִ� ���� ���� �������� ���� ������ ��                                         
                _curCardAmountList.Add(curQuantity);                                             //���ֺ� ���� �־��ֱ�
                temp = devide - curQuantity;                                                  //������ �߰��� ����
                Debug.Log($"�п�ǰ ���� : {_UnitNameList[_NHnum[i]]}, ���� : {_curCardAmountList[i]}");
            }

        }

        List<CardData> haveCardList = DeckDataManagerSO.HaveDeckDataList;
        List<CardNamingType> cardNamingTypes = new List<CardNamingType>();
        List<CardNamingType> NatHaveCardNamingTypes = new List<CardNamingType>();

        private void SetHaveCard()
        {
            int haveCardCount = haveCardList.Count;

            for (int i = 0; i < haveCardCount; i++)
            {
                cardNamingTypes.Add(haveCardList[0]._cardNamingType);
            }
        }
        private void SetNotHaveCard()
        {

        }
        /// <summary>
        /// �ű� ���� ȹ�� �Լ�
        /// </summary>
        private void RandomNewUnit()
        {
            // ������ �ִ� ī�� 
          

 

            // ������ ���� ���� ī�� 
            int cardCount = System.Enum.GetValues(typeof(CardNamingType)).Length;
            bool isNotHave = true; 
            for (int i = 0; i < cardCount; i++ )
            {
                cardNamingTypes.ForEach((x) =>
                {
                    if (x == (CardNamingType)i)
                    {
                        isNotHave = false; 
                    }
                });

                if(isNotHave == true)
                {
                    NatHaveCardNamingTypes.Add((CardNamingType)i);
                    isNotHave = false; 
                }
            }
            

            if (_newPercent >= _newChPercent)                               //ĳ���� Ȯ���� ���� ���ں��� Ŭ���
            {

                //NotHaveUnit();
                CardNamingType cardNamingType; 
                if (_NHnum.Count != 0)
                {
                    cardNamingType = NatHaveCardNamingTypes[Random.Range(0, NatHaveCardNamingTypes.Count)];     //���� ���ֵ��� ���ο� ������ ����
                    UserSaveManagerSO.AddCardData(DeckDataManagerSO.FindStdCardData(cardNamingType));
                    // ������ ���� ���� ����Ʈ �ʱ�ȭ or NatHaveCardNamingTypes.Remove(cardNamingType); 
                    Debug.Log($"���ο� ���� \"{cardNamingType}\"��/�� �������ϴ�.");
                }
            }
            
        } //���� ���߿� ���� �ϴ� �Լ���

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
