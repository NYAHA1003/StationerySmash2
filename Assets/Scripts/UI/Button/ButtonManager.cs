using UnityEngine;
using Util;
using UnityEngine.UI;
using System.Collections.Generic;
//public enum PanelType
//{
//    Sticker,
//    Ttagji,
//    Dalgona
//}
public class ButtonManager : MonoBehaviour
{
    private Btn_MainPanel2 btn_MainPanel;

    [SerializeField]
    private Btn_MainPanel2 btn_MainPanel2;

    [Header("¹öÆ°")]
    [SerializeField]
    private List<Button> cardInfoBtns = new List<Button>();

    [SerializeField]
    private DeckSetting deckSetting; 
    private void Start()
    {
        btn_MainPanel2.Start();
        AddDeckCards(); 
        AddListeners();
    }
    private void AddListeners()
    {
        Debug.Log(deckSetting.deckCards.Count);
        for(int i = 0;i < deckSetting.deckCards.Count; i++)
        {
            cardInfoBtns[i].onClick.AddListener(
           () =>
           {
               OnSkinActive();
           });
        }
    }
    private void AddDeckCards()
    {
        for (int i = 0; i < deckSetting.deckCards.Count; i++)
        {
            cardInfoBtns.Add(deckSetting.deckCards[i].GetComponent<Button>());
        }

    }
    public void OnDeckActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveDeck);   
    }
        
    public void OnDeckDescriptoinActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveCardDescription);
    }

    public void OnSettingActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveSetting);
    }
    public void OnSkinActive()
    {
        EventManager.TriggerEvent(EventsType.ActiveCardInfoPn);
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

    [ContextMenu("s")]
    public void Test()
    {
        EventManager.TriggerEvent(EventsType.SetOriginShopPn);
    }



}
