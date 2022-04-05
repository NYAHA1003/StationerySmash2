using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util;
public class ShopScroll : AgentScroll
{
    [SerializeField]
    private Scrollbar[] yScrollBars;
    [SerializeField]
    private Slider accentSlider;
    [SerializeField]
    private RectTransform[] panelMoveBtns;

    protected override void ChildAwake()
    {
        EventManager.StartListening(EventsType.MoveShopPn, OnMoveShopPanel);
        EventManager.StartListening(EventsType.CloaseAllPn, SetOriginScroll);
        EventManager.StartListening(EventsType.SetOriginShopPn, SetOriginScroll);
    }

    protected override void ChildUpdate()
    {
        accentSlider.value = Mathf.Lerp(accentSlider.value, scrollbar.value, 0.2f);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        SetOriginScroll();

        if (curPos == targetPos)
        {
            DeltaSlide(eventData.delta.x);
            SetOriginScroll();
        }
        ChangeBtnSize();
    }
    #region ��ư �Լ�
    public void OnMoveShopPanel(object n)
    {
        targetIndex = SIZE - (int)n - 1;
        targetPos = pos[(int)n];
        SetOriginScroll();
        ChangeBtnSize();
        Debug.Log(pos[(int)n]);
    }
    #endregion
    /// <summary>
    /// ���� ��ũ�� value �ʱ�ȭ  
    /// </summary>
    void SetOriginScroll()
    {
        Debug.Log("����");
        for (int i = 0; i < SIZE; i++)
        {
            if (contentTr.GetChild(i).GetComponent<ScrollScript>()) //&& pos[i] != curPos && pos[i] == targetPos
            {
                yScrollBars[i].value = 1;
            }
        }
    }

    /// <summary>
    ///  ��ܿ� �г� �ٲٴ� ��ư ũ�� ���� 
    /// </summary>
    void ChangeBtnSize()
    {
        for (int i = 0; i < SIZE; i++)
        {
            panelMoveBtns[i].sizeDelta = new Vector2((targetIndex == SIZE - i - 1) ? 320 : 160, panelMoveBtns[SIZE - i - 1].sizeDelta.y);
        }
    }


}
