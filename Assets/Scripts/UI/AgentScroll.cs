using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    //변수 - 변수별로 무슨 역할인지 주석을 달아주세요
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
    /// Awake에서 사용하는 설정
    /// </summary>
    protected virtual void SettingAwake()
    {
        _scrollbar = transform.GetComponentInChildren<Scrollbar>();
        _contentTrm = _scrollbar.transform.GetChild(0).GetChild(0);
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
    protected virtual void  SettingUpdate()
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
                Debug.Log("타겟인덱스@@" + _targetIndex);
                return _pos[i];
            }
        }
        return 0;
    }
}


