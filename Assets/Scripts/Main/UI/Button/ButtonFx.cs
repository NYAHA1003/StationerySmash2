using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Setting;
using Main.Event;
using Utill.Data;

public class ButtonFx : MonoBehaviour
{
    [SerializeField]
    private EventsType eventType;
    private void Awake()
    {
        EventManager.Instance.StartListening(eventType, (x) => PlaySound((int)x));
    }
    private void PlaySound(int x)
    {
        Sound.PlayEff(x);
    }
}
