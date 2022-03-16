using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ScrollScript : ScrollRect
{

    bool isXmove;
    private NestedScrollBar nestedScrollBar;
    private ScrollRect xScrollRect;

    protected override void Start()
    {
        nestedScrollBar = GameObject.FindWithTag("NestedScrollManager").GetComponent<NestedScrollBar>();
        xScrollRect = GameObject.FindWithTag("NestedScrollManager").GetComponent<ScrollRect>();

    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("실행");
        isXmove = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
        Debug.Log(isXmove);
        if (isXmove)
        {
            nestedScrollBar.OnBeginDrag(eventData);
            xScrollRect.OnBeginDrag(eventData);
        }
        else
        {
            Debug.Log("세로 이동");
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (isXmove)
        {
            nestedScrollBar.OnDrag(eventData);
            xScrollRect.OnDrag(eventData);
        }
        else
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isXmove)
        {
            nestedScrollBar.OnEndDrag(eventData);
            xScrollRect.OnEndDrag(eventData);
        }
        else
            base.OnEndDrag(eventData);
    }
}
