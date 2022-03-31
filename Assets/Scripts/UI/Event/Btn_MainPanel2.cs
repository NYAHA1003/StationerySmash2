using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class Btn_MainPanel2 : MonoBehaviour
{
    [SerializeField,Header("플레이어 덱"),Space(5)]
    private GameObject deck;
    [SerializeField,Header("카드 설명"),Space(5)]
    private GameObject cardDescription;
    [SerializeField,Header("설정 패널"),Space(5)]  
    private GameObject settingPanel;
    void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, OnDeckActive);
        EventManager.StartListening(EventsType.ActiveDescription, OnCardDescriptoinActive);
        EventManager.StartListening(EventsType.ActiveSetting, OnSettingActive);

        //활성화되어 있는 모든 패널 닫기 
        EventManager.StartListening(EventsType.CloaseAllPn, OnDeckDisabled);
        EventManager.StartListening(EventsType.CloaseAllPn, OnCardDescriptionDisabled);
        EventManager.StartListening(EventsType.CloaseAllPn, OnSettingDisabled);
    }

    public void OnDeckActive()
    {
        deck.SetActive(!deck.activeSelf);
    }
    public void OnCardDescriptoinActive()
    {
        cardDescription.SetActive(!cardDescription.activeSelf);
        //cancelPanel.SetActive(true);
    }
    public void OnSettingActive()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void OnDeckDisabled()
    {
        deck.SetActive(false);
    }
    public void OnCardDescriptionDisabled()
    {
        cardDescription.SetActive(false); 
    }
    public void OnSettingDisabled()
    {
        settingPanel.SetActive(false);
    }
}
