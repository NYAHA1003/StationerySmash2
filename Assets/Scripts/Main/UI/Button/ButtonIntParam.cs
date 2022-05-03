using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Event;

public class ButtonIntParam : ButtonAgent
{
    [SerializeField]
    private int intParam; 
    public override void Execute()
    {
        EventManager.TriggerEvent(eventType, intParam);
    }
}
