using UnityEngine;
using Util;
using UnityEngine.UI;
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
    private Button skinPanel; 

    private void Start()
    {
        btn_MainPanel2.Start();
        AddListeners(); 
    }
    
    private void AddListeners()
    {
        skinPanel.onClick.AddListener(
            () =>
            {
                EventManager.TriggerEvent(EventsType.ActiveSkinPn);
                Debug.Log("qq");
                });

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
        EventManager.TriggerEvent(EventsType.ActiveSkinPn);
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
