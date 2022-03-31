using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Util;
public class MainScroll : AgentScroll
{
    [SerializeField]
    private Slider accentSlider;
    [SerializeField]
    private RectTransform[] panelIcons;
    protected override void ChildAwake()
    {
        EventManager.StartListening(EventsType.MoveMainPn, OnMoveMainPanel);
    }
    protected override void ChildStart()
    {
        targetPos = pos[1];
        targetIndex = 1;
        StressImage();
    }
    protected override void ChildeUpdate()
    {
        accentSlider.value = Mathf.Lerp(accentSlider.value, scrollbar.value, 0.2f);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (curPos == targetPos)
        {
            print(eventData.delta.x);

            deltaSlide(eventData.delta.y);
        }
        StressImage();
        EventManager.TriggerEvent(EventsType.SetOriginShopPn);
    }

    private void StressImage()
    {
        for (int i = 0; i < panelIcons.Length; i++)
        {
            if (targetIndex == i)
            {
                panelIcons[i].anchoredPosition3D = new Vector3(-80, 0);
                Debug.Log("현재 타겟 인덱스 : " + targetIndex);
            }
            else panelIcons[i].anchoredPosition3D = new Vector3(0, 0);
        }
    }

    #region 버튼 함수
    public void OnMoveMainPanel(object n)
    {
        targetIndex = SIZE - (int)n - 1;
        targetPos = pos[(int)n];
        StressImage();
    }
    #endregion
}

