using System.Collections.Generic;
using UnityEngine;
using Util;
using System.Linq;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Btn_MainPanel2 _btn_MainPanel2; //메인패널에 사용되는 스크립트 
    private void Start()
    {
        _btn_MainPanel2.Start(); 
    }

    public void OnDeckActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveDeck);
    }

    public void OnDeckDescriptoinActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveDescription);
    }

    public void OnSettingActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveSetting);
    }

    public void OnMoveShopPanel(int iParam)
    {
        EventManager.TriggerEvent(EventsType.MoveShopPn, iParam);
    }

    public void OnMoveMainPanel(int iParam)
    {
        EventManager.TriggerEvent(EventsType.MoveMainPn, iParam);
        EventManager.TriggerEvent(EventsType.CloaseAllPn); 
    }

}
