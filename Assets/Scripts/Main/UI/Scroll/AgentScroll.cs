using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Main.Scroll
{
    /// <summary>
    /// ��ӿ� Ŭ����
    /// </summary>
    public class AgentScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //������Ƽ
        protected int Size
        {
            get => _size;
            set => _size = value;
        }

        //�ν����� ����
        [SerializeField]
        protected int _size = 3;

        //���� ����
        protected Scrollbar _scrollbar = null;
        protected Transform _contentTrm = null;
        protected List<float> _pos = new List<float>();
        [SerializeField]
        private List<IScroll> _scrollObservers = new List<IScroll>();


        //���� - �������� ���� �������� �ּ��� �޾��ּ���
        protected float _distance = 0f;
        protected float _curPos = 0f;
        protected float _targetPos = 0f;
        protected bool _isDrag = false;
        protected int _targetIndex = 0;

        private void Awake()
        {
            SettingAwake();
        }
        private void Start()
        {
            SettingStart();
        }
        private void Update()
        {
            SettingUpdate();
        }

        /// <summary>
        /// �巡�׸� ������ ��
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _curPos = SetPos();
        }
        /// <summary>
        /// �巡�� ���� ��
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            _isDrag = true;
        }
        /// <summary>
        /// �巡�װ� ���� ��
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _isDrag = false;
            _targetPos = SetPos();
        }
        /// <summary>
        /// ������ �߰�
        /// </summary>
        /// <param name="observer"></param>
        public void AddObserver(IScroll observer)
        {
            _scrollObservers.Add(observer);
        }

        /// <summary>
        /// Awake���� ����ϴ� ����
        /// </summary>
        protected virtual void SettingAwake()
        {
            _scrollbar = transform.GetChild(1).GetComponent<Scrollbar>();
            _contentTrm = transform.GetChild(0).GetChild(0);
        }

        /// <summary>
        /// Start���� ����ϴ� ����
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
        /// Update���� ����ϴ� ����
        /// </summary>
        protected virtual void SettingUpdate()
        {
            if (!_isDrag)
            {
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.1f);
            }
        }

        /// <summary>
        /// ��ũ�� �ӵ��� ������ ����
        /// </summary>
        /// <param name="deltaValue"></param>
        protected void DeltaSlide(float deltaValue)
        {
            if (deltaValue > 18 && _curPos - _distance >= 0)
            {
                ++_targetIndex;
                _targetPos = _curPos - _distance;
            }
            else if (deltaValue < -18 && _curPos + _distance <= 1.01f)
            {
                --_targetIndex;
                _targetPos = _curPos + _distance;
            }
            NotifyToObserver();
        }

        /// <summary>
        /// �����ڵ鿡�� ���� �ε����� ����
        /// </summary>
        protected void NotifyToObserver()
        {
            for (int i = 0; i < _scrollObservers.Count; i++)
            {
                _scrollObservers[i].Notify(_targetIndex);
            }
        }

        /// <summary>
        /// ��ũ���� ������ ���� �г� ���� 
        /// </summary>
        /// <returns></returns>
        private float SetPos()
        {
            for (int i = 0; i < Size; i++)
            {
                if (_scrollbar.value < _pos[i] + _distance * 0.5f && _scrollbar.value > _pos[i] - _distance * 0.5f)
                {
                    _targetIndex = Size - i - 1;
                    Debug.Log("Ÿ���ε���@@" + _targetIndex);
                    NotifyToObserver();
                    return _pos[i];
                }
            }
            return 0;
        }
    }
}

