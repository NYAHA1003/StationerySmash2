using Main.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 
public class ButtonButtonTypeParam : ButtonAgent
{
    [SerializeField]
    private ButtonType buttonType;
    public override void Execute()
    {
        for(int i = 0; i < eventTypes.Length; i++)
        {
            EventManager.TriggerEvent(eventTypes[i], buttonType);
        }
    }
}
