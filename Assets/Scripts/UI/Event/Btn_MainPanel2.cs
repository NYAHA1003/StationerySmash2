using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class Btn_MainPanel2 : MonoBehaviour
{
    [SerializeField,Header("�÷��̾� ��"),Space(5)]
    private GameObject deck;
    [SerializeField,Header("ī�� ����"),Space(5)]
    private GameObject cardDescription;
    [SerializeField,Header("���� �г�"),Space(5)]  
    private GameObject settingPanel;
    void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, OnDeckActive);
        EventManager.StartListening(EventsType.ActiveDescription, OnCardDescriptoinActive);
        EventManager.StartListening(EventsType.ActiveSetting, OnSettingActive);

        //Ȱ��ȭ�Ǿ� �ִ� ��� �г� �ݱ� 
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
