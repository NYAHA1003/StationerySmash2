using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
public class BattleComponent
{
    public Dictionary<System.Action, System.Action> _actions = new Dictionary<System.Action, System.Action>();

    /// <summary>
    /// 액션 추가
    /// </summary>
    /// <param name="method"></param>
    /// <param name="addMethod"></param>
    public void AddDictionary(System.Action method, System.Action addMethod)
    {
        if (!_actions.TryGetValue(method, out var name))
        {
            _actions.Add(method, new System.Action(() => { }));

        }
        _actions[method] += addMethod;
    }

    /// <summary>
    /// 액션 사용
    /// </summary>
    /// <param name="method"></param>
    public void RunAction(System.Action method)
    {
        if (_actions.TryGetValue(method, out var name))
        {
            _actions[method].Invoke();
        }
    }
}
