using Main.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Main.Setting;
public class ButtonButtonTypeParam : ButtonAgent
{
    [SerializeField]
    private ButtonType[] buttonType;
    public override void Execute()
    {
        Sound.PlayEff(3);
        for (int i = 0; i < eventTypes.Length; i++)
        {
            EventManager.Instance.TriggerEvent(eventTypes[i], buttonType[i]);
        }
    }
}
