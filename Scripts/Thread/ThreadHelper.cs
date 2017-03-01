using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

public class ThreadHelper : MonoBehaviour
{
    private static List<Action> actions = new List<Action>();
    private static Thread mainThread;
    public static bool IsMainThread()
    {
        return mainThread == null || Thread.CurrentThread == mainThread;
    }
    // Use this for initialization
    void Awake()
    {
        if (mainThread != null)
        {
            Destroy(this);
            return;
        }
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

    public static void QueueOnMainThread(Action action)
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
    public static void QueueOnThreadPool(WaitCallback callBack, object state = null)
    {
        if (callBack == null)
            return;
        ThreadPool.QueueUserWorkItem(callBack, state);
    }
}