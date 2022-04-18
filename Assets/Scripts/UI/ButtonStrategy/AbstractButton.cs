using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractButton : MonoBehaviour
{
    public TempBtnManager tempBtnManager; 

    public  ActiveBehavior activeBehavior;
    public UndoBehavior undoBehavior;

    public void PerformActive(AbstractButton btn)
    {
        btn.gameObject.SetActive(true); 
    }
    public void PerformUndo(AbstractButton btn)
    {
        btn.gameObject.SetActive(false); 
    }
    public abstract AbstractButton SetCurrentBtn();  
}
