using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
[System.Serializable]
public class Btn_MainPanel2
{
    [SerializeField, Header("�÷��̾� ��"), Space(5)]
    private GameObject deck;
    [SerializeField, Header("ī�� ����"), Space(5)]
    private GameObject cardDescription;
    [SerializeField, Header("���� �г�"), Space(5)]
    private GameObject settingPanel;

    public void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, OnDeckActive);
        //EventManager.StartListening(EventsType.ActiveCardDescription, OnCardDescriptoinActive);
        EventManager.StartListening(EventsType.ActiveSetting, OnSettingActive);

        //Ȱ��ȭ�Ǿ� �ִ� ��� �г� �ݱ� 
        EventManager.StartListening(EventsType.CloaseAllPn, OnDeckDisabled);
        EventManager.StartListening(EventsType.CloaseAllPn, OnCardDescriptionDisabled);
        EventManager.StartListening(EventsType.CloaseAllPn, OnSettingDisabled);
    }

    /// <summary>
    /// �÷��̾ Ȱ��ȭ ��Ȱ��ȭ 
    /// </summary>
    public void OnDeckActive()
    {
        deck.SetActive(!deck.activeSelf);
    }
    /// <summary>
    /// ī�弳�� Ȱ��ȭ ��Ȱ��ȭ 
    /// </summary>
    public void OnCardDescriptoinActive()
    {
        cardDescription.SetActive(!cardDescription.activeSelf);
        //cancelPanel.SetActive(true);
    }
    /// <summary>
    /// ����â Ȱ��ȭ ��Ȱ��ȭ 
    /// </summary>
    public void OnSettingActive()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    /// <summary>
    /// �� ��Ȱ��ȭ (��� â �ϰ� ���� �̺�Ʈ���� ���) 
    /// </summary>
    public void OnDeckDisabled()
    {
        deck.SetActive(false);
    }
    /// <summary>
    /// ī�� ����â ��Ȱ��ȭ (��� â �ϰ� ���� �̺�Ʈ���� ���) 
    /// </summary>
    public void OnCardDescriptionDisabled()
    {
        cardDescription.SetActive(false);
    }
    /// <summary>
    /// ����â ��Ȱ��ȭ  (��� â �ϰ� ���� �̺�Ʈ���� ���) 
    /// </summary>
    public void OnSettingDisabled()
    {
        settingPanel.SetActive(false);
    }
}
