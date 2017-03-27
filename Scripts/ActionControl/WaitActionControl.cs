using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public enum WaitActionType
{
    你好
}
public class WaitActionControl : MonoBehaviour {
    private static WaitActionControl instance;
    public static WaitActionControl Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("WaitActionControl");
                instance = obj.AddComponent<WaitActionControl>();
            }
            return instance;
        }
    }
    /// <summary>
    /// 等待动作列表
    /// </summary>
    private List<WaitActionParameter> _waitActionParameterList = new List<WaitActionParameter>();
    /// <summary>
    /// 添加等待动作
    /// </summary>
    /// <param name="type">动作类型</param>
    /// <param name="action">动作</param>
    /// <param name="waitTime">等待时间</param>
    public void AddWaitAction(WaitActionType type,Action action, float waitTime)
    {
        for (int i = 0; i < _waitActionParameterList.Count; i++)
        {
            if (type == _waitActionParameterList[i]._type)
            {
                Debug.LogWarning("重复添加相同的等待动作");
                break;
            }
        }
        WaitActionParameter wp = new WaitActionParameter(type, action, waitTime);
        _waitActionParameterList.Add(wp);
    }
    /// <summary>
    /// 移除等待动作
    /// </summary>
    /// <param name="type">动作类型</param>
    public void RemoveWaitAction(WaitActionType type)
    {
        for (int i = 0; i < _waitActionParameterList.Count; i++)
        {
            if (_waitActionParameterList[i]._type == type)
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
    }
    void FixedUpdate()
    {
        WaitActionParameter wp;
        for (int i = _waitActionParameterList.Count-1; i>=0 ; i--)
        {
            wp = _waitActionParameterList[i];
            if (Time.time - wp._time >= wp._waitTime)
            {
                wp._action();
                RemoveWaitAction(wp._type);
            }
        }
    }

}
