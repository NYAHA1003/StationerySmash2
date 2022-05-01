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
        int _unitAmount = 0;


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
            RandomNewUnit();

            int j = 0;

            _NHnum.Clear();
            for (int i = 0; i < _IsHave.Count; i++)
            {
                if (_IsHave[i])
                {
                    _NHnum[j++] = i;
                }
            }

        }

        /// <summary>
        /// Shiny패키지를 구매할때 실행하는 함수
        /// </summary>
        private void ShinyButtonClick()
        {
            SetPakageSelect(PakageType.ShinyPack);
            RandomNewUnit();
        }

        /// <summary>
        /// Legendary패키지를 구매할때 실행하는 함수
        /// </summary>
        private void LegendaryButtonClick()
        {
            SetPakageSelect(PakageType.LegendaryPack);
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
                    _unitAmount = 20;
                    break;
                case PakageType.ShinyPack:
                    _min = 4; _max = 5;
                    _newPercent = 4;
                    _useDalgona = 10;
                    _unitAmount = 70;
                    break;
                case PakageType.LegendaryPack:
                    _min = 6; _max = 8;
                    _newPercent = 10;
                    _useDalgona = 45;
                    _unitAmount = 300;
                    break;
                default:
                    break;
            }
            _Quantity = Random.Range(_min, _max + 1);
            _newChPercent = Random.Range(0, 100 + 1);
        }

        /// <summary>
        /// 신규 유닛 획득 함수
        /// </summary>
        private void RandomNewUnit()
        {
            int j = 0;
            
            if (_newPercent >= _newChPercent)        //캐릭터 확률이 랜덤 숫자보다 클경우
            {
                for (int i = 0; i < _IsHave.Count; i++)
                {
                    if (!_IsHave[i])
                    {
                        _NHnum[j++] = i;
                    }
                }
            }

            if (_NHnum.Count != 0)
            {
                _newCharacter = _NHnum[Random.Range(0, _NHnum.Count)];
                _IsHave[_newCharacter] = true;      //유닛을 얻었다고 표시를 바꿔줌
            }
        } //제일 나중에 들어가야 하는 함수임
    }
}