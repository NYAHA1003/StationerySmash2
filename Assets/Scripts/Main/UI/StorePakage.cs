using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Utill.Data;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace store
{
    public class StorePakage : MonoBehaviour
    {
        private List<bool> _IsHave = new List<bool>();
        private List<int> _NHnum = new List<int>();
        private List<int> _UnitAmountList = new List<int>();

        //�ӽ� ���� ������ ���� ����
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
        int _CurrentUnitAmount = 0;
        int _unitMaxAmount = 0;


        /// <summary>
        /// �������� �ٲ�� ����
        /// </summary>
        int _Quantity = 0;          //������ �����ȿ��� �������� �������� ���� 
        int _newChPercent = 0;      //���� ����
        int _newCharacter = 0;      //���ο� ĳ���ͷ� ���� ���� ��ȣ

        [SerializeField]
        Button _CommonButton;       //Ŀ�յ�� �п�ǰ ��ȯ��ư
        [SerializeField]        
        Button _ShinyButton;        //���̴ϵ�� �п�ǰ ��ȯ��ư
        [SerializeField]
        Button _LegendaryButton;    //����������� �п�ǰ ��ȯ��ư

        void Start()
        {
            ResetFunctionPakage_UI();
            SetFunctionPakage_UI();
        }
        
        /// <summary>
        /// �̺�Ʈ ����
        /// </summary>
        private void ResetFunctionPakage_UI()
        {
            _CommonButton.onClick.RemoveAllListeners();
            _ShinyButton.onClick.RemoveAllListeners();
            _LegendaryButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// �̺�Ʈ ����
        /// </summary>
        private void SetFunctionPakage_UI()
        {
            _CommonButton.onClick.AddListener(CommonButtonClick);
            _ShinyButton.onClick.AddListener(ShinyButtonClick);
            _LegendaryButton.onClick.AddListener(LegendaryButtonClick);

            SetBoolList();
        }

        /// <summary>
        /// ������ �ִ°Ͱ� ������ ���� ���� �п�ǰ�� �ҷ����� �Լ�
        /// </summary>
        private void SetBoolList()
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
        }

        /// <summary>
        /// Common��Ű���� �����Ҷ� �����ϴ� �Լ�
        /// </summary>
        private void CommonButtonClick()
        {
            SetPakageSelect(PakageType.CommonPack);
            //RandomUnitSummons();
            RandomNewUnit();
        }

        /// <summary>
        /// Shiny��Ű���� �����Ҷ� �����ϴ� �Լ�
        /// </summary>
        private void ShinyButtonClick()
        {
            SetPakageSelect(PakageType.ShinyPack);
            //RandomUnitSummons();
            RandomNewUnit();
        }

        /// <summary>
        /// Legendary��Ű���� �����Ҷ� �����ϴ� �Լ�
        /// </summary>
        private void LegendaryButtonClick()
        {
            SetPakageSelect(PakageType.LegendaryPack);
            //RandomUnitSummons();
            RandomNewUnit();
        }

        /// <summary>
        /// ��Ű���� ������� �������� ���� ����
        /// </summary>
        /// <param name="pack"></param>
        public void SetPakageSelect(PakageType pack)
        {
            switch (pack)
            {
                case PakageType.CommonPack:
                    _min = 2; _max = 3;
                    _newPercent = 1;
                    _useMoney = 500;
                    _unitMaxAmount = 20;
                    _CurrentUnitAmount = _unitMaxAmount;
                    break;
                case PakageType.ShinyPack:
                    _min = 4; _max = 5;
                    _newPercent = 4;
                    _useDalgona = 10;
                    _unitMaxAmount = 70;
                    _CurrentUnitAmount = _unitMaxAmount;
                    break;
                case PakageType.LegendaryPack:
                    _min = 6; _max = 8;
                    _newPercent = 10;
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
        /// ���� ������ ��ȯ�ϴ� �Լ�
        /// </summary>
        /*private void RandomUnitSummons()
        {
            int temp = 0;
            int quantity = 0;
            int devide = _unitMaxAmount / _Quantity;

            _UnitAmountList.Clear();
            _NHnum.Clear();
            HaveUnit();
            Shuffle();

            if(_Quantity > _NHnum.Count)
            {
                _Quantity = _NHnum.Count;
            }

            for (int i = 0; i < _Quantity; i++)                                            //������ �������� ������ ������ŭ ����
            {
                if (i == _Quantity - 1)                                                    //������ �ѿ��� ���� ī�尹����ŭ �־��ش�.
                {
                    _UnitAmountList[i] = _CurrentUnitAmount;
                    return;
                }

                quantity = Random.Range(1 + temp, devide + temp);         //�̹��� ���� ����
                _CurrentUnitAmount -= quantity;                                            //�����ִ� ���� ���� �������� ���� ������ ��
                _UnitAmountList[i] = quantity;                                                    //���ֺ� ���� �־��ֱ� 

                if(temp < 0)
                {
                    temp = 0;
                }

                temp = devide - quantity;                          //������ �߰��� ����

                Debug.Log($"�п�ǰ ��ȣ : {_NHnum[i]}, �п�ǰ ���� : {_UnitAmountList[i]}");
            }
            //���� ��ȯ�Ѱ� ����ϱ� �ؾ���
        }*/

        /// <summary>
        /// �ű� ���� ȹ�� �Լ�
        /// </summary>
        private void RandomNewUnit()
        {
            _NHnum.Clear();
            NotHaveUnit();
            if (_newPercent >= _newChPercent)                               //ĳ���� Ȯ���� ���� ���ں��� Ŭ���
            {
                if (_NHnum.Count != 0)
                {
                    _newCharacter = _NHnum[Random.Range(0, _NHnum.Count)];     //���� ���ֵ��� ���ο� ������ ����
                    _IsHave[_newCharacter] = true;                             //������ ����
                    Debug.Log($"{_newCharacter}�� ĳ���Ͱ� �������ϴ�.");
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

        private void HaveUnit() //�������� ������ �ִ°� ��������?
        {
            int j = -1;

            for (int i = 0; i < _IsHave.Count; i++)
            {
                if (_IsHave[i])
                {
                    j++;
                    _NHnum[j] = i; //������ �ִ� ĳ���͵� ��������
                }
            }
        }
        
        private void NotHaveUnit()
        {
            int j = -1;

            for (int i = 0; i < _IsHave.Count; i++)
            {
                if (!_IsHave[i])
                {
                    j++;
                    _NHnum[j] = i;
                }
            }
        }
    }
}