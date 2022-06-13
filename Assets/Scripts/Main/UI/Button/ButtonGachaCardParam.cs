using Main.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class ButtonGachaCardParam : ButtonAgent
{
    [SerializeField]
    private GachaCard[] _gachaCards;
    public override void Execute()
    {
        for(int i = 0; i < eventTypes.Length; i++)
        {
            EventManager.Instance.TriggerEvent(eventTypes[i], _gachaCards[i]);
        }
    }

}
