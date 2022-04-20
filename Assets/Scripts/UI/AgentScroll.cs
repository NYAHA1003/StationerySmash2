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
    protected Scrollbar scrollbar;
    protected Transform contentTr;

    //protected const int Size = 3;
    protected int size = 3; 
    protected int Size
    {
        get => size;
        set => size = value; 
    }
    //protected float[] pos = new float[Size];
    protected List<float> pos = new List<float>(); 
    protected float distance, curPos, targetPos;
    protected bool isDrag;
    protected int targetIndex;

    private void Awake()
    {
        scrollbar = this.transform.GetChild(1).GetComponent<Scrollbar>();
        contentTr = gameObject.transform.GetChild(0).GetChild(0);
        ChildAwake();
    }
    private void Start()
    {
        distance = 1f / (Size - 1);
        //for (int i = 0; i < Size; i++) pos[i] = distance * i;
        for(int i = 0; i < Size; i++)
        {
            pos.Add(distance * i);
        }
        ChildStart(); 
    }
    private void Update()
    {
        if (!isDrag)
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        ChildUpdate();
    }

    protected virtual void ChildUpdate()
    {
        //nothing 자식용
    }
    protected virtual void ChildAwake()
    {
        //nothing 자식용
    }
    protected virtual void ChildStart()
    {
        //nothing 자식용
    }

    /// <summary>
    /// 스크롤한 정도에 따라 패널 변경 
    /// </summary>
    /// <returns></returns>
    float SetPos()
    {
        for (int i = 0; i < Size; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = Size - i - 1;
                Debug.Log("타겟인덱스@@" + targetIndex);
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
        if (deltaValue > 18 && curPos - distance >= 0)
        {
            ++targetIndex;
            targetPos = curPos - distance;
        }
        else if (deltaValue < -18 && curPos + distance <= 1.01f)
        {
            --targetIndex;
            targetPos = curPos + distance;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        curPos = SetPos();
    }
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();
    }
}


