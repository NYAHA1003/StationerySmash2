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
    /// <summary>
    /// 부모 클래스에서 발동 
    /// </summary>
    protected override void ChildAwake()
    {
        EventManager.StartListening(EventsType.MoveMainPn, OnMoveMainPanel);
    }
    /// <summary>
    /// 부모 클래스에서 발동
    /// </summary>
    protected override void ChildStart()
    {
        targetPos = pos[1];
        targetIndex = 1;
        StressImage();
    }
    /// <summary>
    /// 부모 클래스에서 발동 
    /// </summary>
    protected override void ChildUpdate()
    {
        accentSlider.value = Mathf.Lerp(accentSlider.value, scrollbar.value, 0.2f);
    }

    /// <summary>
    /// 드래그가 끝날시 발동
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (curPos == targetPos)
        {
            print(eventData.delta.x);
            DeltaSlide(eventData.delta.y);
        }
        StressImage();
        EventManager.TriggerEvent(EventsType.SetOriginShopPn);
    }

    /// <summary>
    /// 우측 패널이동 이미지 강조(왼쪽으로 움직임)
    /// </summary>
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
    /// <summary>
    /// 상점,메인,스테이지창 중 한곳으로 이동하는 것 
    /// </summary>
    /// <param name="n">0=상점 1=메인 2=스테이지</param>
    public void OnMoveMainPanel(object n)
    {
        if ((int)n < 0 || (int)n > Size-1)
        {
            Debug.LogError("메인 패널 움직이는 범위 넘어감 0~SIZE-1 사이 값이 아님");
            return;
        }
        targetIndex = Size - (int)n - 1;
        targetPos = pos[(int)n];
        StressImage();
    }
    #endregion
}

