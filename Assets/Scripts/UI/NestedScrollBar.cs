using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    [SerializeField]
    private Transform contentTr;
    [SerializeField]
    private Scrollbar[] yScrollBars;
    [SerializeField]
    private Slider accentSlider;
    [SerializeField]
    private Slider yAccentSlider;
    [SerializeField]
    private RectTransform[] btns;
    [SerializeField]
    private RectTransform[] ySliderIcon; 

    const int SIZE = 3;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex;


    [SerializeField]
    private bool isXSlide;
    void Start()
    {
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
        if(!isXSlide) //ó�� ����
        {
            targetPos = pos[1];
            targetIndex = 1;
            StressImage();
        }
        
    }
    void Update()
    {
        if (isXSlide) accentSlider.value = Mathf.Lerp(accentSlider.value, scrollbar.value, 0.2f);
        else yAccentSlider.value = Mathf.Lerp(yAccentSlider.value, scrollbar.value, 0.2f);
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        }
    }

    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = SIZE - i - 1; 
                Debug.Log("Ÿ���ε���@@" + targetIndex);
                return pos[i];
            }
        }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        curPos = SetPos();
    }
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();
        if (isXSlide) SetOriginScroll();
        if (curPos == targetPos)
        {
            print(eventData.delta.x);
            if (isXSlide)
            {
                deltaSlide(eventData.delta.x);
                SetOriginScroll();
            }
            else
            {
                deltaSlide(eventData.delta.y);
                StressImage(); 
            }
        }
        if (isXSlide)
        {
            ChangeBtnSize(); 
        }
        else
        {
            StressImage();
        }
    }

    /// <summary>
    /// ����ȭ�鿡�� 
    /// </summary>
    void ChangeBtnSize()
    {
        for (int i = 0; i < SIZE; i++)
        {
            btns[i].sizeDelta = new Vector2((targetIndex == SIZE - i - 1) ? 320 : 160, btns[SIZE - i - 1].sizeDelta.y);
        }
    }

    /// <summary>
    /// ����ȭ�鿡�� ���Ϸ� �ѱ�� ��ũ�ѹ� �ٸ� ������ �ѱ�� �ʱ�ȭ
    /// </summary>
    void SetOriginScroll() 
    {
        Debug.Log("����");
        for (int i = 0; i < SIZE; i++)
        {
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && pos[i] != curPos && pos[i] == targetPos)
            {
                yScrollBars[i].value = 1;
            }
        }
    }

    /// <summary>
    /// �����̵� �ӵ��� ���� �г� ��ȭ 
    /// </summary>
    /// <param name="deltaValue"></param>
    void deltaSlide(float deltaValue) 
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

    /// <summary>
    /// ȭ�� ���� �������̹��� ����
    /// </summary>
    void StressImage() 
    {
        for (int i = 0; i < ySliderIcon.Length; i++)
        {
            if (targetIndex == i)
            {
                ySliderIcon[i].anchoredPosition3D = new Vector3(-80, 0);
                Debug.Log("���� Ÿ�� �ε��� : " + targetIndex);
            }
            else ySliderIcon[i].anchoredPosition3D = new Vector3(0, 0);
        }
    }

    #region ��ư �Լ�
    public void OnMoveShopPanel(int n)
    {
        targetIndex = SIZE - n - 1;
        targetPos = pos[n];
        SetOriginScroll();
        ChangeBtnSize();
        Debug.Log(pos[n]);
    }

    public void OnMoveMainPanel(int n)
    {
        targetIndex = SIZE - n - 1;
        targetPos = pos[n];
        StressImage(); 
    }
    #endregion
}
