using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;
public class ShopScroll : AgentScroll
{
    [SerializeField]
    private Scrollbar[] _yScrollBars;
    [SerializeField]
    private Slider _accentSlider;
    [SerializeField]
    private RectTransform[] _panelMoveBtns;
    
    /// <summary>
    /// �θ� Ŭ���� Awake���� ����
    /// </summary>
    protected override void ChildAwake()
    {
        //�̺�Ʈ ���
        EventManager.StartListening(EventsType.MoveShopPn, OnMoveShopPanel);
        EventManager.StartListening(EventsType.CloaseAllPn, SetOriginScroll);
        EventManager.StartListening(EventsType.SetOriginShopPn, SetOriginScroll);
    }
    /// <summary>
    /// �θ� Ŭ���� Start���� ����
    /// </summary>
    protected override void ChildStart() { }
    /// <summary>
    /// �θ� Ŭ���� Update���� ����
    /// </summary>
    protected override void ChildUpdate()
    {
        //�����г� ��� ���������̴� �̵� 
        _accentSlider.value = Mathf.Lerp(_accentSlider.value, _scrollbar.value, 0.2f);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        SetOriginScroll();

        if (_curPos == _targetPos)
        {
            DeltaSlide(eventData.delta.x);
            SetOriginScroll();
        }
        ChangeBtnSize();
    }
    /// <summary>
    /// �����г� �Ϲ�,��Ű��,��ȭ �̵� 
    /// </summary>
    /// <param name="n"></param>
    public void OnMoveShopPanel(object n)
    {
        _targetIndex = SIZE - (int)n - 1;
        _targetPos = pos[(int)n];
        SetOriginScroll();
        ChangeBtnSize();
        Debug.Log(pos[(int)n]);
    }

    /// <summary>
    /// ���� ��ũ�� value �ʱ�ȭ  
    /// </summary>
    private void SetOriginScroll()
    {
        Debug.Log("����");
        for (int i = 0; i < SIZE; i++)
        {
            if (_contentTr.GetChild(i).GetComponent<ScrollScript>()) //&& pos[i] != curPos && pos[i] == targetPos
            {
                _yScrollBars[i].value = 1;
            }
        }
    }

    /// <summary>
    ///  ��ܿ� �г� �ٲٴ� ��ư ũ�� ���� 
    /// </summary>
    private void ChangeBtnSize()
    {
        for (int i = 0; i < SIZE; i++)
        {
            _panelMoveBtns[i].sizeDelta = new Vector2((_targetIndex == SIZE - i - 1) ? 320 : 160, _panelMoveBtns[SIZE - i - 1].sizeDelta.y);
        }
    }


}
