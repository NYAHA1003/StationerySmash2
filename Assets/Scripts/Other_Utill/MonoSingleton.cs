using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// °ÇµéÁö¸¶¼À
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object locker = new object();
    private static T instance = null;
    public static T _instance
    {
        get
        {
            if (shuttingDown)
            {
                return null;
            }

            lock (locker)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        DontDestroyOnLoad(instance);
                    }
                    DontDestroyOnLoad(instance);
                }
            }
            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }
    private void OnDestroy()
    {
        if(instance == this)
        {
            shuttingDown = true;
        }
    }
}
