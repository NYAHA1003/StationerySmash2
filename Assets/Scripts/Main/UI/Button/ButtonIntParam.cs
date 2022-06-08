using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Event;

public class ButtonIntParam : ButtonAgent
{
    [SerializeField]
    private int[] intParam; 
    public override void Execute()
    {
        Debug.Log("rr");
        for(int i = 0; i < eventTypes.Length; i++)
        {
            EventManager.Instance.TriggerEvent(eventTypes[i], intParam[i]);
        }
    }
}
