using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Main.Event
{
    public class EventManager : MonoSingleton<EventManager>
    {

        private Dictionary<EventsType, Action> eventDictionary;
        private Dictionary<EventsType, Action<object>> eventParamDictionary;
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
            if (_instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //���� �̺�Ʈ�� �� ���� �̺�Ʈ �߰� 
                thisEvent += listener;

                //��ųʸ� ������Ʈ
                _instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                //ó������ ��ųʸ��� �̺�Ʈ �߰� 
                thisEvent += listener;
                _instance.eventDictionary.Add(eventName, thisEvent);
            }
        }
        public static void StartListening(EventsType eventName, Action<object> listener)
        {
            Action<object> thisEvent;

            if (_instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += listener;  
                _instance.eventParamDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                _instance.eventParamDictionary.Add(eventName, listener);
            }
        }

        /// <summary>
        /// �̺�Ʈ �Լ� �����ϱ� 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public static void StopListening(EventsType eventName, Action listener)
        {
            if (_instance == null)
            {
                return;
            }
            Action thisEvent;
            if (_instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //���� �̺�Ʈ���� �̺�Ʈ ����
                thisEvent -= listener;

                //�̺�Ʈ ������Ʈ 
                _instance.eventDictionary[eventName] = thisEvent;
            }

        }

        public static void StopListening(EventsType eventName, Action<object> listener)
        {
            if (_instance == null)
            {
                return;
            }
            Action<object> thisEvent;
            if (_instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= listener;

                _instance.eventParamDictionary[eventName] = thisEvent;
            }
        }
        /// <summary>
        /// �̺�Ʈ �Լ� ���� 
        /// </summary>
        /// <param name="eventName"></param>
        public static void TriggerEvent(EventsType eventName)
        {
            Action thisEvent = null;
            if (_instance.eventDictionary.TryGetValue(eventName, out thisEvent))
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
            if (_instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
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