using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Main.Scroll
{
    /// <summary>
    /// 상속용 클래스
    /// </summary>
    public class AgentScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //프로퍼티
        protected int Size
        {
            get => _size;
            set => _size = value;
        }

        //인스펙터 변수
        [SerializeField]
        protected int _size = 3;

        //참조 변수
        protected Scrollbar _scrollbar = null;
        protected Transform _contentTrm = null;
        protected List<float> _pos = new List<float>();
        [SerializeField]
        private List<IScroll> _scrollObservers = new List<IScroll>();


        //변수 - 변수별로 무슨 역할인지 주석을 달아주세요
        protected float _distance = 0f; // scrollbar value, 스크롤바의 패널이 3개면 distance는 0.3333이 된다. 
        protected float _curPos = 0f; // 현재 scrollbar value
        protected float _targetPos = 0f; // 클릭이 끝났을 때의 scrollbar value 
        protected bool _isDrag = false; // 드래그중인가 
        protected static int _targetIndex = 0; // 현재 있는 패널의 인덱스 

        private void Awake()
        {
            SettingAwake();
        }
        private void Start()
        {
            SettingStart();
            OnMoveMainPanel(Size - 1);
        }
        private void Update()
        {
            SettingUpdate();
        }

        /// <summary>
        /// 드래그를 시작할 때
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _curPos = SetPos();
        }
        /// <summary>
        /// 드래그 중일 때
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            _isDrag = true;
        }
        /// <summary>
        /// 드래그가 끝날 때
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _isDrag = false;
            _targetPos = SetPos();
        }
        /// <summary>
        /// 관찰자 추가
        /// </summary>
        /// <param name="observer"></param>
        public void AddObserver(IScroll observer)
        {
            _scrollObservers.Add(observer);
        }

        /// <summary>
        /// Awake에서 사용하는 설정
        /// </summary>
        protected virtual void SettingAwake()
        {
            _scrollbar = transform.GetChild(1).GetComponent<Scrollbar>();
            _contentTrm = transform.GetChild(0).GetChild(0);
        }

        /// <summary>
        /// Start에서 사용하는 설정
        /// </summary>
        protected virtual void SettingStart()
        {
            _distance = 1f / (Size - 1);
            for (int i = 0; i < Size; i++)
            {
                _pos.Add(_distance * i);
            }
        }

        /// <summary>
        /// Update에서 사용하는 설정
        /// </summary>
        protected virtual void SettingUpdate()
        {
            if (!_isDrag)
            {
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.1f);
            }
        }

        /// <summary>
        /// 스크롤 속도가 빠르면 변경
        /// </summary>
        /// <param name="deltaValue"></param>
        protected bool DeltaSlide(float deltaValue)
        {
            if (deltaValue > 18 && _curPos - _distance >= 0)
            {
                ++_targetIndex;
                _targetPos = _curPos - _distance;
                NotifyToObserver();
                return true; 
            }
            else if (deltaValue < -18 && _curPos + _distance <= 1.01f)
            {
                --_targetIndex;
                _targetPos = _curPos + _distance;
                NotifyToObserver();
                return true; 
            }
            return false; 
        }

        /// <summary>
        /// 관찰자들에게 현재 인덱스를 전달
        /// </summary>
        protected void NotifyToObserver()
        {
            for (int i = 0; i < _scrollObservers.Count; i++)
            {
                _scrollObservers[i].Notify(_targetIndex);
            }
        }

        /// <summary>
        /// 스크롤한 정도에 따라 패널 변경 
        /// </summary>
        /// <returns></returns>
        private float SetPos()
        {
            for (int i = 0; i < Size; i++)
            {
                if (_scrollbar.value < _pos[i] + _distance * 0.5f && _scrollbar.value > _pos[i] - _distance * 0.5f)
                {
                    _targetIndex = Size - i - 1;
                    NotifyToObserver();
                    return _pos[i];
                }
            }
            return 0;
        }

        /// <summary>
        /// 패널 이동
        /// </summary>
        /// <param name="index">0=상점 1=메인 2=스테이지</param>
        public virtual void OnMoveMainPanel(int index)
        {
            if (index < 0 || index > Size - 1)
            {
                Debug.LogError("메인 패널 움직이는 범위 넘어감 0~SIZE-1 사이 값이 아님");
                return;
            }
            _targetIndex = Size - index - 1;
            _targetPos = _pos[index];
        }
    }
}

