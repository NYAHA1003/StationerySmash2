using UnityEngine;
using Util; 

//public enum PanelType
//{
//    Sticker,
//    Ttagji,
//    Dalgona
//}
public class ButtonManager : MonoBehaviour
{
    private Btn_MainPanel2 btn_MainPanel;
    //[Header("메인화면 UI")]
    //[SerializeField]
    //private GameObject deck;
    //[SerializeField]
    //private GameObject cardDescription;
    //[SerializeField]
    //private GameObject cancelPanel;
    //[SerializeField]
    //private GameObject settingPanel; 

    [SerializeField]
    private Btn_MainPanel2 btn_MainPanel2;
    private void Start()
    {
        // btn_MainPanel = new Btn_MainPanel(this, deck, cardDescription, cancelPanel, settingPanel);
        btn_MainPanel2.Start(); 
    }

    #region 이벤트 매니저 사용하려 했던거 
    /*
    public void OnActiveDescription()
    {
        EventManager.TriggerEvent("ActiveDescription");
        Debug.Log("이벤트매니저");
    }
    private void OnSetUnitUpgradeInfo()
    {
        StoreUnitInfo stricker = EventSystem.current.currentSelectedGameObject.GetComponent<StoreUnitInfo>();
        Debug.Log(stricker.name);
        textUnitName.text = stricker.unitName;
        textUpgradeInfo.text = stricker.upgradeInfo;
        textCost.text = string.Format("가격 {0} 원", stricker.cost);
    }
    public void OnDescriptionActive() 
    {
        descriptionPanel.SetActive(!descriptionPanel.activeSelf);
    }
    */
    #endregion

    #region 메인화면 UI버튼함수
    public void OnDeckActive()
    {
        //btn_MainPanel.OnDeckActive();
        EventManager.TriggerEvent(EventsType.ActiveDeck);   
    }

    public void OnDeckDescriptoinActive()
    {
        //btn_MainPanel.OnDeckDescriptoinActive();
        EventManager.TriggerEvent(EventsType.ActiveCardDescription);
    }

    public void OnSettingActive()
    {
        //btn_MainPanel.OnSettingActive();
        EventManager.TriggerEvent(EventsType.ActiveSetting);
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
    #endregion
}
