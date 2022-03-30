using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;
using static UnityEngine.Debug;

//public enum PanelType
//{
//    Sticker,
//    Ttagji,
//    Dalgona
//}
public class ButtonManager : MonoBehaviour
{

    private Btn_MainPanel btn_MainPanel;



    [SerializeField]
    private TextMeshProUGUI textUnitName;
    [SerializeField]
    private TextMeshProUGUI textUpgradeInfo;
    [SerializeField]
    private TextMeshProUGUI textCost;
    [SerializeField]
    private GameObject descriptionPanel;

    private StringBuilder _stringbuilder = new StringBuilder();

    //[Header("메인화면 UI")]
    //[SerializeField]
    //private GameObject deck;
    //[SerializeField]
    //private GameObject cardDescription;
    //[SerializeField]
    //private GameObject cancelPanel;
    //[SerializeField]
    //private GameObject settingPanel; 
    private void Start()
    {
        // btn_MainPanel = new Btn_MainPanel(this, deck, cardDescription, cancelPanel, settingPanel);
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
        EventManager.TriggerEvent(EventType.ActiveDeck);
    }

    public void OnDeckDescriptoinActive()
    {
        //btn_MainPanel.OnDeckDescriptoinActive();
        EventManager.TriggerEvent(EventType.ActiveDescription);
    }

    public void OnSettingActive()
    {
        //btn_MainPanel.OnSettingActive();
        EventManager.TriggerEvent(EventType.ActiveSetting);
    }

    public void OnMoveShopPanel(int iParam)
    {
        EventManager.TriggerEvent(EventType.MoveShopPn, iParam);
    }

    public void OnMoveMainPanel(int iParam)
    {
        EventManager.TriggerEvent(EventType.MoveMainPn, iParam);
    }
    #endregion
}
