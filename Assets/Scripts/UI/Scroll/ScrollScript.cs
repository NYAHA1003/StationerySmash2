using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ScrollScript : ScrollRect
{
    [SerializeField]
    private bool isHorizontalMove = false; //이 스크롤뷰타 수직으로 이동할 때, 그냥 인스펙터에서 안 보임 Debug로 열어서 수정
    
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
        // 드래그 시작하는 순간 수평이동이 크면 부모가 드래그 시작한 것, 수직이동이 크면 자식이 드래그 시작한 것

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
