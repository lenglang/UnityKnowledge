using UnityEngine;
using System.Collections;
using System;

public class LoopActionParameter
{
    public Action _action;
    public float _time;
    public float _interval;
    public LoopActionType _type;
    public int _times;
    public float _interval2;
    public LoopActionParameter(LoopActionType type, Action action, float interval, int times, float interval2)
    {
        _action = action;
        _interval = interval;
        _time = Time.time;
        _type = type;
        _times = times;
        _interval2 = interval2;
    }
}

