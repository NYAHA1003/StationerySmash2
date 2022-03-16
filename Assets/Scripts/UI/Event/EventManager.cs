using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventParam
{
    public string str;
    public int i;
    public float f;
    public bool b; 
}
public class EventManager : MonoBehaviour
{

    private Dictionary<EventType, Action> eventDictionary;
    private Dictionary<EventType, Action<EventParam>> eventParamDictionary; 

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("씬안에 이벤트매니저 없음");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventType, Action>();
        }
        if(eventParamDictionary == null)
        {
            eventParamDictionary = new Dictionary<EventType, Action<EventParam>>(); 
        }
    }

    public static void StartListening(EventType eventName, Action listener)
    {
        Action thisEvent;  
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //기존 이벤트에 더 많은 이벤트 추가 
            thisEvent += listener;

            //딕셔너리 업데이트
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //처음으로 딕셔너리에 이벤트 추가 
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(EventType eventName,Action<EventParam> listner)
    {
        Action<EventParam> thisEvent; 
        //스크립트1에서 StartListening("1", abc); 스크립트2에서 StartListening("1", qwe); 이런식으로 다른 여러곳에서 하나의 이벤트에 등록?하려면  
        //StartListening 을 하나의 eventName에 여러번 하게 되잖아 
        //해봤을 때 이게 되는데 왜 되는지 잘 이해가 안가 
        //함수 호출시 마다 thisEvent 액션 변수가 새로 선언이 되는데 
        //밑 if문에서 thisEvnet += listener 하면 텅빈 thisEvent에 하나의 Listner만 들어가는게 맞지 않나 
        if(instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listner;

            instance.eventParamDictionary[eventName] = thisEvent; 
        }
        else
        {
            thisEvent += listner;
            instance.eventParamDictionary.Add(eventName, listner);
        }
    }

    public static void StopListening(EventType eventName, Action listener)
    {
        if (eventManager == null) return;
        Action thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //기존 이벤트에서 이벤트 제거
            thisEvent -= listener;

            //이벤트 업데이트 
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void StopListening(EventType eventName, Action<EventParam> listener)
    {
        if (eventManager == null) return;
        Action<EventParam> thisEvent; 
        if(instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;

            instance.eventParamDictionary[eventName] = thisEvent; 
        }
    }
    public static void TriggerEvent(EventType eventName)
    {
        Action thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //thisEvent.Invoke();
            instance.eventDictionary[eventName]();
            //OR USE instance.eventDictionary[eventName]();
        }
    }
}