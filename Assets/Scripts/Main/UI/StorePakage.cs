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

        //임시 보유 데이터 선언 시작
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
        int _CurrentUnitAmount = 0;
        int _unitMaxAmount = 0;


        /// <summary>
        /// 랜덤으로 바뀌는 정보
        /// </summary>
        int _Quantity = 0;          //정해진 범위안에서 랜덤으로 정해지는 수량 
        int _newChPercent = 0;      //랜덤 숫자
        int _newCharacter = 0;      //새로운 캐릭터로 뽑힌 유닛 번호

        [SerializeField]
        Button _CommonButton;       //커먼등급 학용품 소환버튼
        [SerializeField]        
        Button _ShinyButton;        //샤이니등급 학용품 소환버튼
        [SerializeField]
        Button _LegendaryButton;    //레전더리등급 학용품 소환버튼

        void Start()
        {
            ResetFunctionPakage_UI();
            SetFunctionPakage_UI();
        }
        
        /// <summary>
        /// 이벤트 리셋
        /// </summary>
        private void ResetFunctionPakage_UI()
        {
            _CommonButton.onClick.RemoveAllListeners();
            _ShinyButton.onClick.RemoveAllListeners();
            _LegendaryButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// 이벤트 세팅
        /// </summary>
        private void SetFunctionPakage_UI()
        {
            _CommonButton.onClick.AddListener(CommonButtonClick);
            _ShinyButton.onClick.AddListener(ShinyButtonClick);
            _LegendaryButton.onClick.AddListener(LegendaryButtonClick);

            SetBoolList();
        }

        /// <summary>
        /// 가지고 있는것과 가지고 있지 않은 학용품을 불러오는 함수
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
        /// Common패키지를 구매할때 실행하는 함수
        /// </summary>
        private void CommonButtonClick()
        {
            SetPakageSelect(PakageType.CommonPack);
            //RandomUnitSummons();
            RandomNewUnit();
        }

        /// <summary>
        /// Shiny패키지를 구매할때 실행하는 함수
        /// </summary>
        private void ShinyButtonClick()
        {
            SetPakageSelect(PakageType.ShinyPack);
            //RandomUnitSummons();
            RandomNewUnit();
        }

        /// <summary>
        /// Legendary패키지를 구매할때 실행하는 함수
        /// </summary>
        private void LegendaryButtonClick()
        {
            SetPakageSelect(PakageType.LegendaryPack);
            //RandomUnitSummons();
            RandomNewUnit();
        }

        /// <summary>
        /// 패키지를 골랐을때 여러가지 값을 대입
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
        /// 랜덤 유닛을 소환하는 함수
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

            for (int i = 0; i < _Quantity; i++)                                            //위에서 랜덤으로 정해진 수량만큼 실행
            {
                if (i == _Quantity - 1)                                                    //마지막 팩에는 남은 카드갯수만큼 넣어준다.
                {
                    _UnitAmountList[i] = _CurrentUnitAmount;
                    return;
                }

                quantity = Random.Range(1 + temp, devide + temp);         //이번에 나올 수량
                _CurrentUnitAmount -= quantity;                                            //남아있는 뽑힐 유닛 갯수에서 나온 수량을 뺌
                _UnitAmountList[i] = quantity;                                                    //유닛별 수량 넣어주기 

                if(temp < 0)
                {
                    temp = 0;
                }

                temp = devide - quantity;                          //다음에 추가될 수량

                Debug.Log($"학용품 번호 : {_NHnum[i]}, 학용품 수량 : {_UnitAmountList[i]}");
            }
            //유닛 소환한거 출력하기 해야함
        }*/

        /// <summary>
        /// 신규 유닛 획득 함수
        /// </summary>
        private void RandomNewUnit()
        {
            _NHnum.Clear();
            NotHaveUnit();
            if (_newPercent >= _newChPercent)                               //캐릭터 확률이 랜덤 숫자보다 클경우
            {
                if (_NHnum.Count != 0)
                {
                    _newCharacter = _NHnum[Random.Range(0, _NHnum.Count)];     //없는 유닛들중 새로운 유닛을 선택
                    _IsHave[_newCharacter] = true;                             //유닛을 생성
                    Debug.Log($"{_newCharacter}번 캐릭터가 뽑혔습니다.");
                }
            }
            
        } //제일 나중에 들어가야 하는 함수임

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

        private void HaveUnit() //수량보다 가지고 있는게 적을때는?
        {
            int j = -1;

            for (int i = 0; i < _IsHave.Count; i++)
            {
                if (_IsHave[i])
                {
                    j++;
                    _NHnum[j] = i; //가지고 있는 캐릭터들 가져오기
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