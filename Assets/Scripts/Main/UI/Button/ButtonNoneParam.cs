using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Event;
public class ButtonNoneParam : ButtonAgent
{
    public override void Execute()
    {
        for(int i = 0; i < eventTypes.Length;i++)
        {
            EventManager.TriggerEvent(eventTypes[i]);
        }
    }

}
