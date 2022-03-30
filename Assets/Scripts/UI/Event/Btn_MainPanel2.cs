using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_MainPanel2 : MonoBehaviour
{
    [SerializeField]
    private GameObject deck;
    [SerializeField]
    private GameObject cardDescription;
    [SerializeField]
    private GameObject cancelPanel;
    [SerializeField]
    private GameObject settingPanel;

    void Start()
    {
        EventManager.StartListening(EventType.ActiveDeck, OnDeckActive);
        EventManager.StartListening(EventType.ActiveDescription, OnDeckDescriptoinActive);
        EventManager.StartListening(EventType.ActiveSetting, OnSettingActive);

        //활성화되어 있는 모든 패널 닫기 
        EventManager.StartListening(EventType.CloaseAllPn, OnDeckActive);
        EventManager.StartListening(EventType.CloaseAllPn, OnDeckDescriptoinActive);
        EventManager.StartListening(EventType.CloaseAllPn, OnSettingActive);
    }

    public void OnDeckActive()
    {
        if(deck)
        {
            deck.SetActive(false);
            return; 
        }
        deck.SetActive(true);
        //deck.SetActive(!deck.activeSelf);
    }
    public void OnDeckDescriptoinActive()
    {
        if(cardDescription)
        {
            cardDescription.SetActive(false);
            return; 
        }
        cardDescription.SetActive(true);
        //cancelPanel.SetActive(true);
    }
    bool isBtnActive; 
    public void OnSettingActive()
    {
        if (settingPanel)
        {
            settingPanel.SetActive(false); 
            return;
        }
        settingPanel.SetActive(true);
    }
}
