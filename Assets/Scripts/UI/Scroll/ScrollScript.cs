using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ScrollScript : ScrollRect
{
    [SerializeField]
    private bool isHorizontalMove = false; //�� ��ũ�Ѻ�Ÿ �������� �̵��� ��, �׳� �ν����Ϳ��� �� ���� Debug�� ��� ����
    
    bool forParent;
    AgentScroll agentScroll;
    ScrollRect parentScrollRect;

    protected override void Start()
    {
        agentScroll = GetComponentInParent<AgentScroll>();
        parentScrollRect = agentScroll.GetComponent<ScrollRect>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� �����ϴ� ���� �����̵��� ũ�� �θ� �巡�� ������ ��, �����̵��� ũ�� �ڽ��� �巡�� ������ ��

        if (isHorizontalMove)
        {
            forParent = Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x);
        }
        else
        {
            forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
        }

        if (forParent)
        {
            agentScroll.OnBeginDrag(eventData);
            parentScrollRect.OnBeginDrag(eventData);
        }
        else base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            agentScroll.OnDrag(eventData);
            parentScrollRect.OnDrag(eventData);
        }
        else base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            agentScroll.OnEndDrag(eventData);
            parentScrollRect.OnEndDrag(eventData);
        }
        else base.OnEndDrag(eventData);
    }
}
