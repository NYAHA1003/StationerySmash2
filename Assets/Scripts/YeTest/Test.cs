using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using static UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject s;

    private void Start()
    {
        Addressables.LoadAssetAsync<GameObject>("Slow").Completed +=
            (AsyncOperationHandle<GameObject> obj) =>
            {
                s = obj.Result;
                Addressables.Release(obj);
            };
        Func();
    }
    public async void Func()
    {
        EffectObject a = s.GetComponent<EffectObject>();
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Slow");
        await handle.Task;
    }
}