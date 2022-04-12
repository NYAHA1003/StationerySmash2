using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class Userdata1 : MonoBehaviour, ILoader<int,int>
{
    public Dictionary<int, int> MakeDict()
    {
        throw new System.NotImplementedException();
    }
    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        //   TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}"); // text 파일이 textAsset에 담긴다. TextAsset 타입은 텍스트파일 에셋이라고 생각하면 됨!
        //   return JsonUtility.FromJson<Loader>(textAsset.text);
        return JsonUtility.FromJson<Loader>(name);
    }
}
