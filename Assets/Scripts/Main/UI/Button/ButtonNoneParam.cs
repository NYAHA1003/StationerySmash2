using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Event;
using Main.Setting;
public class ButtonNoneParam : ButtonAgent
{
    public override void Execute()
    {
        Debug.Log("dd");
        Sound.PlayEff(Utill.Data.EffSoundType.Spawn);
        for(int i = 0; i < eventTypes.Length; ++i)
        {
            EventManager.Instance.TriggerEvent(eventTypes[i]);
        }
    }

}
