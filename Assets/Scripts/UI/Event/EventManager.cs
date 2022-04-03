using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class EventParam
{
    public string str;
    public int i;
    public float f;
    public bool b; 
}

public class EventManager : MonoBehaviour
{

    private Dictionary<EventsType, Action> eventDictionary;
    private Dictionary<EventsType, Action<object>> eventParamDictionary; 

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
                    Debug.LogError("���ȿ� �̺�Ʈ�Ŵ��� ����");
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
            eventDictionary = new Dictionary<EventsType, Action>();
        }
        if(eventParamDictionary == null)
        {
            eventParamDictionary = new Dictionary<EventsType, Action<object>>(); 
        }

    }

    public static void StartListening(EventsType eventName, Action listener)
    {
        Action thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //���� �̺�Ʈ�� �� ���� �̺�Ʈ �߰� 
            thisEvent += listener;

            //��ųʸ� ������Ʈ
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //ó������ ��ųʸ��� �̺�Ʈ �߰� 
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(EventsType eventName,Action<object> listener)
    {
        Action<object> thisEvent; 
       
        if(instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            instance.eventParamDictionary[eventName] = thisEvent; 
        }
        else
        {
            thisEvent += listener;
            instance.eventParamDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(EventsType eventName, Action listener)
    {
        if (eventManager == null) return;
        Action thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //���� �̺�Ʈ���� �̺�Ʈ ����
            thisEvent -= listener;

            //�̺�Ʈ ������Ʈ 
            instance.eventDictionary[eventName] = thisEvent;
        }

    }

    public static void StopListening(EventsType eventName, Action<object> listener)
    {
        if (eventManager == null) return;
        Action<object> thisEvent; 
        if(instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;

            instance.eventParamDictionary[eventName] = thisEvent; 
        }
    }
    public static void TriggerEvent(EventsType eventName)
    {
        Action thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
            //OR USE instance.eventDictionary[eventName]();
        }
    }

    public static void TriggerEvent(EventsType eventName, object param)
    {
        Action<object> thisEvent = null;
        if (instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
            //OR USE instance.eventDictionary[eventName]();
        }
    }
}