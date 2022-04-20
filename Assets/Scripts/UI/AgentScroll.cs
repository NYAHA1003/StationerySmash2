using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ��ӿ� Ŭ����
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
        //nothing �ڽĿ�
    }
    protected virtual void ChildAwake()
    {
        //nothing �ڽĿ�
    }
    protected virtual void ChildStart()
    {
        //nothing �ڽĿ�
    }

    /// <summary>
    /// ��ũ���� ������ ���� �г� ���� 
    /// </summary>
    /// <returns></returns>
    float SetPos()
    {
        for (int i = 0; i < Size; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = Size - i - 1;
                Debug.Log("Ÿ���ε���@@" + targetIndex);
                return pos[i];
            }
        }
        return 0;
    }
    /// <summary>
    /// ��ũ�� �ӵ��� ������ ����
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


