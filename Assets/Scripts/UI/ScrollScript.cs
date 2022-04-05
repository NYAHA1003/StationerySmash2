using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ScrollScript : ScrollRect
{

    bool isXmove;
    private ShopScroll mainUIScrollBar;
    private ScrollRect xScrollRect;

    protected override void Start()
    {
        mainUIScrollBar = FindObjectOfType<ShopScroll>().GetComponent<ShopScroll>();
        xScrollRect = FindObjectOfType<ShopScroll>().GetComponent<ScrollRect>();

    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("실행");
        isXmove = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
        Debug.Log(isXmove);
        if (isXmove)
        {
            mainUIScrollBar.OnBeginDrag(eventData);
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
            mainUIScrollBar.OnDrag(eventData);
            xScrollRect.OnDrag(eventData);
        }
        else
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isXmove)
        {
            mainUIScrollBar.OnEndDrag(eventData);
            xScrollRect.OnEndDrag(eventData);
        }
        else
            base.OnEndDrag(eventData);
    }
}
