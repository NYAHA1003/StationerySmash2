using Main.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using UnityEngine.UI; 
public abstract class ButtonAgent : MonoBehaviour
{
    public EventsType eventType;

    private void Start()
    {   
        gameObject.GetComponent<Button>().onClick.AddListener(() => Execute());
    }
    public abstract void Execute();
}
