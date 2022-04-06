using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 상속용 클래스
/// </summary>
public abstract class AgentScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Scrollbar _scrollbar;
    protected Transform _contentTr;

    protected const int SIZE = 3;
    protected float[] pos = new float[SIZE];
    protected float _distance, _curPos, _targetPos;
    protected bool _isDrag;
    protected int _targetIndex;

    private void Awake()
    {
        _scrollbar = this.transform.GetChild(1).GetComponent<Scrollbar>();
        _contentTr = gameObject.transform.GetChild(0).GetChild(0);
        ChildAwake();
    }
    private void Start()
    {
        _distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = _distance * i;
        ChildStart(); 
    }
    private void Update()
    {
        if (!_isDrag)
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.1f);
        ChildUpdate();
    }

    protected abstract void ChildUpdate();
    protected abstract void ChildAwake();
    protected abstract void ChildStart();

    /// <summary>
    /// 스크롤한 정도에 따라 패널 변경 
    /// </summary>
    /// <returns></returns>
    private float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (_scrollbar.value < pos[i] + _distance * 0.5f && _scrollbar.value > pos[i] - _distance * 0.5f)
            {
                _targetIndex = SIZE - i - 1;
                Debug.Log("타겟인덱스@@" + _targetIndex);
                return pos[i];
            }
        }
        return 0;
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        _curPos = SetPos();
    }
    public void OnDrag(PointerEventData eventData)
    {
        _isDrag = true;
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        _targetPos = SetPos();
    }
}


