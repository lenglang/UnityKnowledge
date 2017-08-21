using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
namespace WZK
{
    public class ThreadHelper : MonoBehaviour
    {
        private List<Action> actions = new List<Action>();
        private Thread mainThread = null;
        private static ThreadHelper _instance;
        public static ThreadHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (new GameObject("ThreadHelper")).AddComponent<ThreadHelper>();
                }
                return _instance;
            }
        }
        public bool IsMainThread()
        {
            return mainThread == null || Thread.CurrentThread == mainThread;
        }
        // Use this for initialization
        void Awake()
        {
            mainThread = Thread.CurrentThread;
            DontDestroyOnLoad(gameObject);
        }
        // Update is called once per frame
        void Update()
        {
            var currentActions = new List<Action>();
            lock (actions)
            {
                currentActions.AddRange(actions);
                foreach (var item in currentActions)
                    actions.Remove(item);
            }
            foreach (var action in currentActions)
            {
                if (action != null)
                    action();
            }
        }
        public void QueueOnMainThread(Action action)
        {
            if (action == null)
                return;
            if (IsMainThread())
            {
                action();
                return;
            }
            lock (actions)
            {
                actions.Add(action);
            }
        }
        public void QueueOnThreadPool(WaitCallback callBack, object state = null)
        {
            if (callBack == null)
                return;
            ThreadPool.QueueUserWorkItem(callBack, state);
        }
    }
}