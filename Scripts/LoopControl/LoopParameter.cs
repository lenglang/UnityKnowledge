using UnityEngine;
using System.Collections;
using System;

public class LoopParameter
{
    public Action _action;
    public float _time;
    public float _interval;
    public string _actionName;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="actionName">动作名</param>
    /// <param name="action">动作</param>
    /// <param name="interval">间隔</param>
    public LoopParameter(string actionName, Action action, float interval)
    {
        _actionName = actionName;
        _action = action;
        _interval = interval;
        _time = Time.time;
    }
}

