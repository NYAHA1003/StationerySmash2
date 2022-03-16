using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_MainPanel : ButtonCommand  
{
    private GameObject deck;
    private GameObject cardDescription;
    private GameObject cancelPanel;
    private GameObject settingPanel;

    public Btn_MainPanel(ButtonManager buttonManager,GameObject deck, GameObject cardDescription, GameObject cancelPanel, GameObject settingPanel) : base(buttonManager)
    {
        this.deck = deck;
        this.cardDescription = cardDescription;
        this.cancelPanel = cancelPanel;
        this.settingPanel = settingPanel; 
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
