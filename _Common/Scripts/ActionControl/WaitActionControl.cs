using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Timers;
/// <summary>
/// 等待动作控制
/// 注意全局的话，要针对某个事件在OnDestroy移除，不是全局的话，移除所以事件
/// </summary>
/// <typeparam name="T"></typeparam>
public class WaitActionControl<T>
{
    private static WaitActionControl<T> instance;
    public static WaitActionControl<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WaitActionControl<T>();
            }
            return instance;
        }
    }
    /// <summary>
    /// 等待动作列表
    /// </summary>
    private List<WaitActionParameter<T>> _waitActionParameterList = new List<WaitActionParameter<T>>();
    /// <summary>
    /// 添加等待动作
    /// </summary>
    /// <param name="type">动作类型</param>
    /// <param name="action">动作</param>
    /// <param name="waitTime">等待时间</param>
    public void AddWaitAction(Action action, float waitTime, T type = default(T))
    {
        WaitActionParameter<T> wp = new WaitActionParameter<T>(type, action, waitTime);
        _waitActionParameterList.Add(wp);
    }
    /// <summary>
    /// 移除等待动作
    /// </summary>
    /// <param name="type">动作类型</param>
    public void RemoveWaitAction(T type)
    {
        for (int i = 0; i < _waitActionParameterList.Count; i++)
        {
            if (type.Equals(_waitActionParameterList[i]._type))
            {
                _waitActionParameterList.RemoveAt(i);
                break;
            }
        }
    }
    /// <summary>
    /// 移除所有等待动作
    /// </summary>
    public void RemoveAllWaitAction()
    {
        _waitActionParameterList.Clear();
        instance = null;
    }
    public void FixedUpdate()
    {
        if (_waitActionParameterList.Count == 0) return;
        WaitActionParameter<T> wp;
        for (int i = _waitActionParameterList.Count - 1; i >= 0; i--)
        {
            wp = _waitActionParameterList[i];
            if (Time.time - wp._time >= wp._waitTime)
            {
                wp._action();
                _waitActionParameterList.RemoveAt(i);
            }
        }
    }
}
public class WaitActionParameter<T>
{
    public Action _action;
    public float _time;
    public float _waitTime;
    public T _type;
    public WaitActionParameter(T type, Action action, float waitTime)
    {
        _action = action;
        _waitTime = waitTime;
        _time = Time.time;
        _type = type;
    }
}
