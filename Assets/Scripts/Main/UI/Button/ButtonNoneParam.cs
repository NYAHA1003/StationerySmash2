using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Event;
public class ButtonNoneParam : ButtonAgent
{
    public override void Execute()
    {
        EventManager.TriggerEvent(eventType);
    }
}
