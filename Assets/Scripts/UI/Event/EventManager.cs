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
    public static void StartListening(EventType eventName,Action<EventParam> listner)
    {
        Action<EventParam> thisEvent; 
        //��ũ��Ʈ1���� StartListening("1", abc); ��ũ��Ʈ2���� StartListening("1", qwe); �̷������� �ٸ� ���������� �ϳ��� �̺�Ʈ�� ���?�Ϸ���  
        //StartListening �� �ϳ��� eventName�� ������ �ϰ� ���ݾ� 
        //�غ��� �� �̰� �Ǵµ� �� �Ǵ��� �� ���ذ� �Ȱ� 
        //�Լ� ȣ��� ���� thisEvent �׼� ������ ���� ������ �Ǵµ� 
        //�� if������ thisEvnet += listener �ϸ� �ֺ� thisEvent�� �ϳ��� Listner�� ���°� ���� �ʳ� 
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
            //���� �̺�Ʈ���� �̺�Ʈ ����
            thisEvent -= listener;

            //�̺�Ʈ ������Ʈ 
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