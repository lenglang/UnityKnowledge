using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public enum LoopActionType
{
    雨水播放
}
public class LoopActionControl : MonoBehaviour
{
    private static LoopActionControl instance;
    public static LoopActionControl Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("LoopActionControl");
                instance = obj.AddComponent<LoopActionControl>();
            }
            return instance;
        }
    }
    /// <summary>
    /// 循环动作列表
    /// </summary>
    private List<LoopActionParameter> _loopActionParameterList = new List<LoopActionParameter>();
    /// <summary>
    /// 添加循环动作
    /// </summary>
    /// <param name="type">循环类型</param>
    /// <param name="action">动作</param>
    /// <param name="interval">循环间隔</param>
    /// <param name="isDoNow">是否马上执行放还是隔几秒后执行，默认马上执行</param>
    /// <param name="times">循环次数，默认0即无限循环</param>
    /// <param name="interval2">第一次间隔结束后，是否改变之后的间隔时间，默认0即不改变，其他值为下次间隔时间</param>
    public void AddLoopAction(LoopActionType type, Action action, float interval, bool isDoNow = true,int times=0,float interval2=0)
    {
        for (int i = 0; i < _loopActionParameterList.Count; i++)
        {
            if (type == _loopActionParameterList[i]._type)
            {
                Debug.LogWarning("重复添加相同的循环动作");
                break;
            }
        }
        LoopActionParameter lp = new LoopActionParameter(type, action, interval, times,interval2);
        _loopActionParameterList.Add(lp);
        if (isDoNow) action();
    }
    /// <summary>
    /// 移除某个循环动作
    /// </summary>
    /// <param name="actionName"></param>
    public void RemoveLoopAction(LoopActionType type)
    {
        for (int i = 0; i < _loopActionParameterList.Count; i++)
        {
            if (_loopActionParameterList[i]._type == type)
            {
                _loopActionParameterList.RemoveAt(i);
                break;
            }
        }
    }
    /// <summary>
    /// 移除所有循环动作
    /// </summary>
    public void RemoveAllLoopAction()
    {
        _loopActionParameterList.Clear();
    }
    void FixedUpdate()
    {
        LoopActionParameter lp;
        for (int i = _loopActionParameterList.Count - 1; i >= 0; i--)
        {
            lp = _loopActionParameterList[i];
            if (Time.time - lp._time >= lp._interval)
            {
                lp._action();
                lp._time = Time.time;
                if (lp._interval2 != 0 && lp._interval != lp._interval2) lp._interval = lp._interval2;
                if (lp._times>0)
                {
                    lp._times--;
                    if (lp._times == 0) RemoveLoopAction(lp._type);
                }
            }
        }
    }
    
}
