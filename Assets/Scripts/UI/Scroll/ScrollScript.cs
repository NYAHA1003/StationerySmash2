using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ScrollScript : ScrollRect
{
    [SerializeField]
    private bool isHorizontalMove = false; //�� ��ũ�Ѻ�Ÿ �������� �̵��� ��, �׳� �ν����Ϳ��� �� ���� Debug�� ��� ����
    [SerializeField]
    private bool isAutoSizeInContent = false; //�������� �ִ� �ڽ� ���� ���� ��ũ�Ѻ� ũ�Ⱑ �޶���

    bool forParent;
    AgentScroll agentScroll;
    ScrollRect parentScrollRect;

    protected override void Start()
    {
        agentScroll = GetComponentInParent<AgentScroll>();
        parentScrollRect = agentScroll.GetComponent<ScrollRect>();

        if (isAutoSizeInContent)
        {
            var rect = GetComponent<RectTransform>();
            float size = 0;

            if (isHorizontalMove)
            {
                size = content.transform.childCount * content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
                rect.sizeDelta = new Vector2(size, rect.sizeDelta.y);
            }
            else
            {
                size = content.transform.childCount * content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, size);
            }

        }
    }
    //

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
