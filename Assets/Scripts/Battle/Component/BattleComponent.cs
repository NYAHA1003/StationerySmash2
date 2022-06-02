using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
using System;
public class BattleComponent
{
    public Dictionary<string, Action<object>> _actions = new Dictionary<string, Action<object>>();
    public Dictionary<Action, Action> _voidActions = new Dictionary<Action, Action>();

    /// <summary>
    /// 액션 추가
    /// </summary>
    /// <param name="method"></param>
    /// <param name="addMethod"></param>
    public void AddDictionary<T>(Action<T> method, Action<T> addMethod)
    {
        var methodName = method.ToString();

        if (!_actions.TryGetValue(methodName, out var name))
        {
            _actions.Add(methodName, new Action<object>((x) => {}));

        }
        _actions[methodName] += Convert(addMethod);
    }

    /// <summary>
    /// 매개변수가 없는 액션 추가
    /// </summary>
    /// <param name="method"></param>
    /// <param name="addMethod"></param>
    public void AddDictionary(Action method, Action addMethod)
    {
        if (!_voidActions.TryGetValue(method, out var name))
        {
            _voidActions.Add(method, new Action(() => { }));

        }
        _voidActions[method] += addMethod;
    }

    /// <summary>
    /// 액션 사용
    /// </summary>
    /// <param name="method"></param>
    public void RunAction<T>(Action<T> method, T data)
    {
        var methodName = method.ToString();

        _actions[method.ToString()].Invoke(data);

        if (_actions.TryGetValue(methodName, out var name))
        {
            _actions[methodName].Invoke(data);
        }
    }

    /// <summary>
    /// 매개변수가 없는 액션 사용
    /// </summary>
    /// <param name="method"></param>
    public void RunAction(Action method)
    {
        if (_voidActions.TryGetValue(method, out var name))
        {
            _voidActions[method].Invoke();
        }
    }

    /// <summary>
    /// 액션T 형을 액션object로 변경한다
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="myActionT"></param>
    /// <returns></returns>
    private Action<object> Convert<T>(Action<T> myActionT)
    {
        if (myActionT == null) return null;
        else return new Action<object>(o => myActionT((T)o));
    }
}
