using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///通知中心
/// </summary>
public class NotificationCenter
{
    /// <summary>
    /// 定义事件分发委托
    /// </summary>
    public delegate void OnNotification(object param=null);
    /// <summary>
    /// 通知中心单例
    /// </summary>
    private static NotificationCenter _instance = null;
    public static NotificationCenter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NotificationCenter();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 存储事件的字典
    /// </summary>
    private Dictionary<string, OnNotification> eventListeners= new Dictionary<string, OnNotification>();
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    /// <param name="eventListener">事件监听器</param>
    public void AddEventListener(string eventKey, OnNotification eventListener)
    {
        if (!eventListeners.ContainsKey(eventKey))
        {
            eventListeners.Add(eventKey, eventListener);
        }
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    public void RemoveEventListener(string eventKey)
    {
        if (!eventListeners.ContainsKey(eventKey))
            return;

        eventListeners[eventKey] = null;
        eventListeners.Remove(eventKey);
    }
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    /// <param name="notific">通知</param>
    public void DispatchEvent(string eventKey)
    {
        if (!eventListeners.ContainsKey(eventKey))
            return;
        eventListeners[eventKey]();
    }
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="eventKey">事件Key</param>
    /// <param name="param">通知内容</param>
    public void DispatchEvent(string eventKey, object param)
    {
        if (!eventListeners.ContainsKey(eventKey))
            return;
        eventListeners[eventKey](param);
    }

    /// <summary>
    /// 是否存在指定事件的监听器
    /// </summary>
    public Boolean HasEventListener(string eventKey)
    {
        return eventListeners.ContainsKey(eventKey);
    }

}

