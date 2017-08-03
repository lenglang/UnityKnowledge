using System;
using System.Collections.Generic;
using UnityEngine.Events;
/// <summary>
/// 通知控制
/// </summary>
/// <typeparam name="T1">枚举类型</typeparam>
public class NotificationControl<T1>
{
    /// <summary>
    /// 通知中心单例
    /// </summary>
    private static NotificationControl<T1> _instance = null;
    public static NotificationControl<T1> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NotificationControl<T1>();
                if (!typeof(T1).IsEnum)
                {
                    throw new ArgumentException("传递的类型请使用枚举");
                }
            }
            return _instance;
        }
    }
    /// <summary>
    /// 存储事件的字典
    /// </summary>
    private Dictionary<T1, UnityEvent> _eventDictionary = new Dictionary<T1, UnityEvent>();
    /// <summary>
    /// 添加监听事件委托
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    /// <param name="eventListener">事件监听器</param>
    public void AddEventListener(T1 eventKey, UnityAction action)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
        {
            UnityEvent myEvent = new UnityEvent();
            myEvent.AddListener(action);
            _eventDictionary.Add(eventKey, myEvent);
        }
        else
        {
            _eventDictionary[eventKey].AddListener(action);
        }
    }
    /// <summary>
    /// 移除某个事件
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    public void RemoveEvent(T1 eventKey)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
            return;
        _eventDictionary[eventKey] = null;
        _eventDictionary.Remove(eventKey);
    }
    /// <summary>
    /// 移除所有事件
    /// </summary>
    public void RemoveAllEvent()
    {
        _eventDictionary.Clear();
        _instance = null;
    }
    /// <summary>
    /// 移除监听委托
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(T1 eventKey, UnityAction action)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
            return;
        _eventDictionary[eventKey].RemoveListener(action);
    }
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventKey"></param>
    public void DispatchEvent(T1 eventKey)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
            return;
        _eventDictionary[eventKey].Invoke();
    }
    /// <summary>
    /// 是否存在指定事件
    /// </summary>
    public Boolean HasEvent(T1 eventKey)
    {
        return _eventDictionary.ContainsKey(eventKey);
    }
}
/// <summary>
/// 通知控制
/// </summary>
/// <typeparam name="T1">枚举类型</typeparam>
/// <typeparam name="T2">传递参数</typeparam>
public class NotificationControl<T1, T2>
{
    /// <summary>
    /// 通知中心单例
    /// </summary>
    private static NotificationControl<T1, T2> _instance = null;
    public static NotificationControl<T1, T2> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NotificationControl<T1, T2>();
                if (!typeof(T1).IsEnum)
                {
                    throw new ArgumentException("传递的类型请使用枚举");
                }
            }
            return _instance;
        }
    }
    public class MyEvent : UnityEvent<T2> { }
    /// <summary>
    /// 存储事件的字典
    /// </summary>
    private Dictionary<T1, MyEvent> _eventDictionary = new Dictionary<T1, MyEvent>();
    public void AddEventListener(T1 eventKey, UnityAction<T2> action)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
        {
            MyEvent myEvent = new MyEvent();
            myEvent.AddListener(action);
            _eventDictionary.Add(eventKey, myEvent);
        }
        else
        {
            _eventDictionary[eventKey].AddListener(action);
        }
    }
    /// <summary>
    /// 移除某个事件
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    public void RemoveEvent(T1 eventKey)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
            return;
        _eventDictionary[eventKey] = null;
        _eventDictionary.Remove(eventKey);
    }
    /// <summary>
    /// 移除所有事件
    /// </summary>
    public void RemoveAllEvent()
    {
        _eventDictionary.Clear();
        _instance = null;
    }
    /// <summary>
    /// 移除监听函数
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="sendData"></param>
    public void RemoveEventListener(T1 eventKey, UnityAction<T2> action)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
            return;
        _eventDictionary[eventKey].RemoveListener(action);
    }
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="receiveData"></param>
    public void DispatchEvent(T1 eventKey, T2 data)
    {
        if (!_eventDictionary.ContainsKey(eventKey))
            return;
        _eventDictionary[eventKey].Invoke(data);
    }
    /// <summary>
    /// 是否存在指定事件
    /// </summary>
    public Boolean HasEvent(T1 eventKey)
    {
        return _eventDictionary.ContainsKey(eventKey);
    }
}

