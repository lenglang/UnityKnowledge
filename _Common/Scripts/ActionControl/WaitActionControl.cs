using UnityEngine;
using System.Collections.Generic;
using System;
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
    /// 等待动作字典
    /// </summary>
    private Dictionary<T, WaitActionParameter> _waitActionDictionary = new Dictionary<T, WaitActionParameter>();
    /// <summary>
    /// 添加等待动作
    /// </summary>
    /// <param name="type">动作类型</param>
    /// <param name="action">动作</param>
    /// <param name="waitTime">等待时间</param>
    public void AddWaitAction(Action action, float waitTime, T type = default(T))
    {
        if (HasKey(type)) return;
        WaitActionParameter wp = new WaitActionParameter(action, waitTime);
        _waitActionDictionary.Add(type,wp);
    }
    /// <summary>
    /// 是否包含Key
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool HasKey(T type)
    {
        return _waitActionDictionary.ContainsKey(type);
    }
    /// <summary>
    /// 移除等待动作
    /// </summary>
    /// <param name="type">动作类型</param>
    public void RemoveWaitAction(T type)
    {
        if (HasKey(type) == false) return;
        _waitActionDictionary.Remove(type);
    }
    /// <summary>
    /// 移除所有等待动作
    /// </summary>
    public void RemoveAllWaitAction()
    {
        _waitActionDictionary.Clear();
        instance = null;
    }
    public void FixedUpdate()
    {
        WaitActionParameter wp;
        foreach (KeyValuePair<T,WaitActionParameter> item in _waitActionDictionary)
        {
            wp = item.Value;
            if (Time.time - wp._time >= wp._waitTime)
            {
                wp._action();
                _waitActionDictionary.Remove(item.Key);
            }
        }
    }
}
public class WaitActionParameter
{
    public Action _action;
    public float _time;
    public float _waitTime;
    public WaitActionParameter(Action action, float waitTime)
    {
        _action = action;
        _waitTime = waitTime;
        _time = Time.time;
    }
}
