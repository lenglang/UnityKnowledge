using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class LoopControl : MonoBehaviour
{
    //public class LoopDo
    //{
    //    public static string 雨水播放 = "雨水播放";
    //}
    private static LoopControl instance;
    public static LoopControl Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("LoopControl");
                instance = obj.AddComponent<LoopControl>();
            }
            return instance;
        }
    }
    private List<LoopParameter> _loopParameterList = new List<LoopParameter>();
    /// <summary>
    /// 添加循环执行动作
    /// </summary>
    /// <param name="action">动作</param>
    /// <param name="interval">间隔时间</param>
    ///<param name="isDoFirst">是否执行第一次</param>
    public void AddLoopAction(string actionName, Action action, float interval, bool isDoFirst = true)
    {
        LoopParameter lp = new LoopParameter(actionName, action, interval);
        _loopParameterList.Add(lp);
        if (isDoFirst) action();
    }
    /// <summary>
    /// 移除某个循环动作
    /// </summary>
    /// <param name="actionName"></param>
    public void RemoveLoopAction(string actionName)
    {
        LoopParameter lp;
        for (int i = 0; i < _loopParameterList.Count; i++)
        {
            lp = _loopParameterList[i];
            if (lp._actionName == actionName)
            {
                _loopParameterList.RemoveAt(i);
                break;
            }
        }
    }
    /// <summary>
    /// 移除所有循环动作
    /// </summary>
    public void RemoveAllLoopAction()
    {
        _loopParameterList.Clear();
    }
    void FixedUpdate()
    {
        LoopParameter lp;
        for (int i = _loopParameterList.Count - 1; i >= 0; i--)
        {
            lp = _loopParameterList[i];
            if (Time.time - lp._time >= lp._interval)
            {
                lp._action();
                lp._time = Time.time;
            }
        }
    }
}
