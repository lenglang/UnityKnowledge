using UnityEngine;
using System.Collections;
using System;

public class WaitActionParameter
{
    public Action _action;
    public float _time;
    public float _waitTime;
    public WaitActionType _type;
    public WaitActionParameter(WaitActionType type, Action action, float waitTime)
    {
        _action = action;
        _waitTime = waitTime;
        _time = Time.time;
        _type = type;
    }
}

