using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Main.Event
{
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
                        eventManager.Initialize();
                    }
                }

                return eventManager;
            }
        }

        void Initialize()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<EventsType, Action>();
            }
            if (eventParamDictionary == null)
            {
                eventParamDictionary = new Dictionary<EventsType, Action<object>>();
            }

        }

        /// <summary>
        /// �̺�Ʈ �Լ� ����ϱ� 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
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
        public static void StartListening(EventsType eventName, Action<object> listener)
        {
            Action<object> thisEvent;

            if (instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
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

        /// <summary>
        /// �̺�Ʈ �Լ� �����ϱ� 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public static void StopListening(EventsType eventName, Action listener)
        {
            if (eventManager == null)
            {
                return;
            }
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
            if (eventManager == null)
            {
                return;
            }
            Action<object> thisEvent;
            if (instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= listener;

                instance.eventParamDictionary[eventName] = thisEvent;
            }
        }
        /// <summary>
        /// �̺�Ʈ �Լ� ���� 
        /// </summary>
        /// <param name="eventName"></param>
        public static void TriggerEvent(EventsType eventName)
        {
            Action thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                Debug.Log("�̺�Ʈ ����!");
                thisEvent?.Invoke();
            }
            else
            {
                Debug.LogError("�� �̺�Ʈ�Դϴ�");
            }
        }

        public static void TriggerEvent(EventsType eventName, object param)
        {
            Action<object> thisEvent = null;
            if (instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent?.Invoke(param);
            }
            else
            {
                Debug.LogError("�� �̺�Ʈ�Դϴ�");
            }
        }
    }
}