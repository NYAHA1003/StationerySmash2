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
    private Slider _accentSlider; //���� �г� ���������̴� 
    [SerializeField]
    private RectTransform[] _panelIcons; //�г� ������
    /// <summary>
    /// �θ� Ŭ�������� �ߵ� 
    /// </summary>
    protected override void ChildAwake()
    {
        EventManager.StartListening(EventsType.MoveMainPn, OnMoveMainPanel);
    }
    /// <summary>
    /// �θ� Ŭ�������� �ߵ�
    /// </summary>
    protected override void ChildStart()
    {
        _targetPos = pos[1];
        _targetIndex = 1;
        StressImage();
    }
    /// <summary>
    /// �θ� Ŭ�������� �ߵ� 
    /// </summary>
    protected override void ChildUpdate()
    {
        _accentSlider.value = Mathf.Lerp(_accentSlider.value, _scrollbar.value, 0.2f);
    }

    /// <summary>
    /// �巡�װ� ������ �ߵ�
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (_curPos == _targetPos)
        {
            print(eventData.delta.x);
            DeltaSlide(eventData.delta.y);
        }
        StressImage();
        EventManager.TriggerEvent(EventsType.SetOriginShopPn);
    }

    /// <summary>
    /// ���� �г��̵� �̹��� ����(�������� ������)
    /// </summary>
    private void StressImage()
    {
        for (int i = 0; i < _panelIcons.Length; i++)
        {
            if (_targetIndex == i)
            {
                _panelIcons[i].anchoredPosition3D = new Vector3(-80, 0);
                Debug.Log("���� Ÿ�� �ε��� : " + _targetIndex);
            }
            else _panelIcons[i].anchoredPosition3D = new Vector3(0, 0);
        }
    }

    #region ��ư �Լ�
    /// <summary>
    /// ����,����,��������â �� �Ѱ����� �̵��ϴ� �� 
    /// </summary>
    /// <param name="n">0=���� 1=���� 2=��������</param>
    public void OnMoveMainPanel(object n)
    {
        if ((int)n < 0 || (int)n > SIZE-1)
        {
            Debug.LogError("���� �г� �����̴� ���� �Ѿ 0~SIZE-1 ���� ���� �ƴ�");
            return;
        }
        _targetIndex = SIZE - (int)n - 1;
        _targetPos = pos[(int)n];
        StressImage();
    }
    #endregion
}

