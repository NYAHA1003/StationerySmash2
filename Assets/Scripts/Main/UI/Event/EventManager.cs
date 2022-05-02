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
        /// 이벤트 함수 등록하기 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public static void StartListening(EventsType eventName, Action listener)
        {
            Action thisEvent;
            if (_instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //기존 이벤트에 더 많은 이벤트 추가 
                thisEvent += listener;

                //딕셔너리 업데이트
                _instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                //처음으로 딕셔너리에 이벤트 추가 
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
        /// 이벤트 함수 해제하기 
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
                //기존 이벤트에서 이벤트 제거
                thisEvent -= listener;

                //이벤트 업데이트 
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
        /// 이벤트 함수 실행 
        /// </summary>
        /// <param name="eventName"></param>
        public static void TriggerEvent(EventsType eventName)
        {
            Action thisEvent = null;
            if (_instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                Debug.Log("이벤트 실행!");
                thisEvent?.Invoke();
            }
            else
            {
                Debug.LogError("빈 이벤트입니다");
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
                Debug.LogError("빈 이벤트입니다");
            }
        }
    }
}