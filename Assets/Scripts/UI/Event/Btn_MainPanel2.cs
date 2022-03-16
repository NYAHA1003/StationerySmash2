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
    }

    public void OnDeckActive()
    {
        deck.SetActive(!deck.activeSelf);
    }
    public void OnDeckDescriptoinActive()
    {
        cardDescription.SetActive(!cardDescription.activeSelf);
        cancelPanel.SetActive(true);
    }
    public void OnSettingActive()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }
}
