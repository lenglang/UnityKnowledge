using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace WZK.Common
{
    /// <summary>
    /// 循环动作控制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoopActionControl<T>
    {
        private static LoopActionControl<T> instance;
        public static LoopActionControl<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoopActionControl<T>();
                }
                return instance;
            }
        }
        /// <summary>
        /// 循环动作列表
        /// </summary>
        private List<LoopActionParameter<T>> _loopActionParameterList = new List<LoopActionParameter<T>>();
        /// 添加循环动作
        /// </summary>
        /// <param name="action">动作</param>
        /// <param name="interval">循环间隔</param>
        /// <param name="type">该动作类型</param>
        /// <param name="isDoNow">是否马上执行放还是隔几秒后执行，默认马上执行</param>
        /// <param name="interval2">第一次间隔结束后，是否改变之后的间隔时间，默认0即不改变，其他值为下次间隔时间</param>
        /// <param name="times">循环次数，默认-1即无限循环</param>
        public void AddLoopAction(Action action, float interval, T type=default(T),bool isDoNow = true, float interval2 = 0, int times = -1)
        {
            LoopActionParameter<T> lp = new LoopActionParameter<T>(type, action, interval, times, interval2);
            _loopActionParameterList.Add(lp);
            if (isDoNow) action();
        }
        /// <summary>
        /// 移除某个循环动作
        /// </summary>
        /// <param name="actionName"></param>
        public void RemoveLoopAction(T type)
        {
            for (int i = 0; i < _loopActionParameterList.Count; i++)
            {
                if (type.Equals(_loopActionParameterList[i]._type))
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
            instance = null;
        }
        public void FixedUpdate()
        {
            if (_loopActionParameterList.Count == 0) return;
            LoopActionParameter<T> lp;
            for (int i = _loopActionParameterList.Count - 1; i >= 0; i--)
            {
                lp = _loopActionParameterList[i];
                if (Time.time - lp._time >= lp._interval)
                {
                    lp._action();
                    lp._time = Time.time;
                    if (lp._interval2 != 0 && lp._interval != lp._interval2) lp._interval = lp._interval2;
                    if (lp._times > 0)
                    {
                        lp._times--;
                        if (lp._times == 0) _loopActionParameterList.RemoveAt(i);
                    }
                }
            }
        }
    }
    public class LoopActionParameter<T>
    {
        public Action _action;
        public float _time;
        public float _interval;
        public T _type;
        public int _times;
        public float _interval2;
        public LoopActionParameter(T type, Action action, float interval, int times, float interval2)
        {
            _action = action;
            _interval = interval;
            _time = Time.time;
            _type = type;
            _times = times;
            _interval2 = interval2;
        }
    }
}
