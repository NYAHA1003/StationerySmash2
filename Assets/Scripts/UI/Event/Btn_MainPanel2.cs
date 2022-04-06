using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
[System.Serializable]
public class Btn_MainPanel2
{
    [SerializeField, Header("�÷��̾� ��"), Space(5)]
    private GameObject _deck;
    [SerializeField, Header("ī�� ����"), Space(5)]
    private GameObject _cardDescription;
    [SerializeField, Header("���� �г�"), Space(5)]
    private GameObject _settingPanel;

    public void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, OnDeckActive);
        EventManager.StartListening(EventsType.ActiveDescription, OnCardDescriptoinActive);
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
        _deck.SetActive(!_deck.activeSelf);
    }
    /// <summary>
    /// ī�弳�� Ȱ��ȭ ��Ȱ��ȭ 
    /// </summary>
    public void OnCardDescriptoinActive()
    {
        _cardDescription.SetActive(!_cardDescription.activeSelf);
        //cancelPanel.SetActive(true);
    }
    /// <summary>
    /// ����â Ȱ��ȭ ��Ȱ��ȭ 
    /// </summary>
    public void OnSettingActive()
    {
        _settingPanel.SetActive(!_settingPanel.activeSelf);
    }

    /// <summary>
    /// �� ��Ȱ��ȭ (��� â �ϰ� ���� �̺�Ʈ���� ���) 
    /// </summary>
    public void OnDeckDisabled()
    {
        _deck.SetActive(false);
    }
    /// <summary>
    /// ī�� ����â ��Ȱ��ȭ (��� â �ϰ� ���� �̺�Ʈ���� ���) 
    /// </summary>
    public void OnCardDescriptionDisabled()
    {
        _cardDescription.SetActive(false);
    }
    /// <summary>
    /// ����â ��Ȱ��ȭ  (��� â �ϰ� ���� �̺�Ʈ���� ���) 
    /// </summary>
    public void OnSettingDisabled()
    {
        _settingPanel.SetActive(false);
    }
}
