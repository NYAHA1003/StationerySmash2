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
    /// 부모 클래스 Awake에서 실행
    /// </summary>
    protected override void ChildAwake()
    {
        //이벤트 등록
        EventManager.StartListening(EventsType.MoveShopPn, OnMoveShopPanel);
        EventManager.StartListening(EventsType.CloaseAllPn, SetOriginScroll);
        EventManager.StartListening(EventsType.SetOriginShopPn, SetOriginScroll);
    }
    /// <summary>
    /// 부모 클래스 Start에서 실행
    /// </summary>
    protected override void ChildStart() { }
    /// <summary>
    /// 부모 클래스 Update에서 실행
    /// </summary>
    protected override void ChildUpdate()
    {
        //상점패널 상단 강조슬라이더 이동 
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
    /// 상점패널 일반,패키지,재화 이동 
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
    /// 상점 스크롤 value 초기화  
    /// </summary>
    private void SetOriginScroll()
    {
        Debug.Log("실행");
        for (int i = 0; i < SIZE; i++)
        {
            if (_contentTr.GetChild(i).GetComponent<ScrollScript>()) //&& pos[i] != curPos && pos[i] == targetPos
            {
                _yScrollBars[i].value = 1;
            }
        }
    }

    /// <summary>
    ///  상단에 패널 바꾸는 버튼 크기 변경 
    /// </summary>
    private void ChangeBtnSize()
    {
        for (int i = 0; i < SIZE; i++)
        {
            _panelMoveBtns[i].sizeDelta = new Vector2((_targetIndex == SIZE - i - 1) ? 320 : 160, _panelMoveBtns[SIZE - i - 1].sizeDelta.y);
        }
    }


}
