using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
[System.Serializable]
public class Btn_MainPanel2
{
    [SerializeField, Header("플레이어 덱"), Space(5)]
    private GameObject deck;
    [SerializeField, Header("카드 설명"), Space(5)]
    private GameObject cardDescription;
    [SerializeField, Header("설정 패널"), Space(5)]
    private GameObject settingPanel;

    public void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, OnDeckActive);
        //EventManager.StartListening(EventsType.ActiveCardDescription, OnCardDescriptoinActive);
        EventManager.StartListening(EventsType.ActiveSetting, OnSettingActive);

        //활성화되어 있는 모든 패널 닫기 
        EventManager.StartListening(EventsType.CloaseAllPn, OnDeckDisabled);
        EventManager.StartListening(EventsType.CloaseAllPn, OnCardDescriptionDisabled);
        EventManager.StartListening(EventsType.CloaseAllPn, OnSettingDisabled);
    }

    /// <summary>
    /// 플레이어덱 활성화 비활성화 
    /// </summary>
    public void OnDeckActive()
    {
        deck.SetActive(!deck.activeSelf);
    }
    /// <summary>
    /// 카드설명 활성화 비활성화 
    /// </summary>
    public void OnCardDescriptoinActive()
    {
        cardDescription.SetActive(!cardDescription.activeSelf);
        //cancelPanel.SetActive(true);
    }
    /// <summary>
    /// 설정창 활성화 비활성화 
    /// </summary>
    public void OnSettingActive()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    /// <summary>
    /// 덱 비활성화 (모든 창 일괄 종료 이벤트에서 사용) 
    /// </summary>
    public void OnDeckDisabled()
    {
        deck.SetActive(false);
    }
    /// <summary>
    /// 카드 설명창 비활성화 (모든 창 일괄 종료 이벤트에서 사용) 
    /// </summary>
    public void OnCardDescriptionDisabled()
    {
        cardDescription.SetActive(false);
    }
    /// <summary>
    /// 설정창 비활성화  (모든 창 일괄 종료 이벤트에서 사용) 
    /// </summary>
    public void OnSettingDisabled()
    {
        settingPanel.SetActive(false);
    }
}
