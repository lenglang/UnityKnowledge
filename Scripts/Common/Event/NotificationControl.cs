using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WZK.Common
{
    /// <summary>
    /// 作者-wzk
    /// 功能-消息分发
    /// 说明-代码或物体被删除，仍然会执行里面的方法.....，所以添加监听方法的时候，务必在OnDestroy里将监听的方法移除，控制的好，在OnDestroy移除所有RemoveAllEvent
    /// </summary>
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
                }
                return _instance;
            }
        }
        /// <summary>
        /// 存储事件的字典
        /// </summary>
        private Dictionary<T1, List<Action>> _eventListeners = new Dictionary<T1, List<Action>>();

        /// <summary>
        /// 添加监听事件委托
        /// </summary>
        /// <param name="eventKey">事件Key</param>
        /// <param name="eventListener">事件监听器</param>
        public void AddEventListener(T1 eventKey, Action action)
        {
            if (!_eventListeners.ContainsKey(eventKey))
            {
                List<Action> list = new List<Action>();
                list.Add(action);
                _eventListeners.Add(eventKey, list);
            }
            else
            {
                if (_eventListeners[eventKey].IndexOf(action) == -1)
                {
                    _eventListeners[eventKey].Add(action);
                }
                else
                {
                    Debug.LogError("重复添加该委托函数");
                }
            }
        }
        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventKey">事件Key</param>
        public void RemoveEvent(T1 eventKey)
        {
            if (!_eventListeners.ContainsKey(eventKey))
                return;
            _eventListeners[eventKey] = null;
            _eventListeners.Remove(eventKey);
        }
        /// <summary>
        /// 移除所有事件
        /// </summary>
        public void RemoveAllEvent()
        {
            _eventListeners.Clear();
            _instance = null;
        }
        /// <summary>
        /// 移除监听委托
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(T1 eventKey, Action action)
        {
            if (!_eventListeners.ContainsKey(eventKey))
                return;
            for (int i = 0; i < _eventListeners[eventKey].Count; i++)
            {
                if (_eventListeners[eventKey][i] == action)
                {
                    _eventListeners[eventKey].RemoveAt(i);
                    break;
                }
            }
        }
        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventKey"></param>
        public void DispatchEvent(T1 eventKey)
        {
            if (!_eventListeners.ContainsKey(eventKey))
                return;
            for (int i = _eventListeners[eventKey].Count - 1; i >= 0; i--)
            {
                if (_eventListeners[eventKey][i] == null) Debug.Log("xxxxxxx");
                try
                {
                    _eventListeners[eventKey][i]();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    _eventListeners[eventKey].RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// 是否存在指定事件
        /// </summary>
        public Boolean HasEvent(T1 eventKey)
        {
            return _eventListeners.ContainsKey(eventKey);
        }
    }
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
                }
                return _instance;
            }
        }
        public delegate void ReceiveData(T2 data);

        /// <summary>
        /// 存储事件的字典
        /// </summary>
        private Dictionary<T1, List<ReceiveData>> _eventListeners = new Dictionary<T1, List<ReceiveData>>();

        public void AddEventListener(T1 eventKey, ReceiveData receiveData)
        {
            if (!_eventListeners.ContainsKey(eventKey))
            {
                List<ReceiveData> list = new List<ReceiveData>();
                list.Add(receiveData);
                _eventListeners.Add(eventKey, list);
            }
            else
            {
                if (_eventListeners[eventKey].IndexOf(receiveData) == -1)
                {
                    _eventListeners[eventKey].Add(receiveData);
                }
                else
                {
                    Debug.LogError("重复添加该委托函数");
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventKey">事件Key</param>
        public void RemoveEvent(T1 eventKey)
        {
            if (!_eventListeners.ContainsKey(eventKey))
                return;
            _eventListeners[eventKey] = null;
            _eventListeners.Remove(eventKey);
        }

        /// <summary>
        /// 移除所有事件
        /// </summary>
        public void RemoveAllEvent()
        {
            _eventListeners.Clear();
            _instance = null;
        }
        /// <summary>
        /// 移除监听函数
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="sendData"></param>
        public void RemoveEventListener(T1 eventKey, ReceiveData receiveData)
        {
            if (!_eventListeners.ContainsKey(eventKey))
                return;
            for (int i = 0; i < _eventListeners[eventKey].Count; i++)
            {
                if (_eventListeners[eventKey][i] == receiveData)
                {
                    _eventListeners[eventKey].RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="receiveData"></param>
        public void DispatchEvent(T1 eventKey, T2 receiveData)
        {
            if (!_eventListeners.ContainsKey(eventKey))
                return;
            for (int i = _eventListeners[eventKey].Count - 1; i >= 0; i--)
            {
                try
                {
                    _eventListeners[eventKey][i](receiveData);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    _eventListeners[eventKey].RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 是否存在指定事件
        /// </summary>
        public Boolean HasEvent(T1 eventKey)
        {
            return _eventListeners.ContainsKey(eventKey);
        }
    }
}

