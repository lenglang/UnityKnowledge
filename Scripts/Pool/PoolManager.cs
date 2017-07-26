using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyPoolManager  {
    private static MyPoolManager _instance;
    public static MyPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MyPoolManager();
            }
            return _instance;
        }
    }
    private static string poolConfigPathPrefix = "Assets/UnityKnowledge/Resources/";
    private const string poolConfigPathMiddle = "gameobjectpool";
    private const string poolConfigPathPostfix = ".asset";
    public static string PoolConfigPath
    {
        get
        {
            return poolConfigPathPrefix + poolConfigPathMiddle + poolConfigPathPostfix;
        }
    }
    public static string PoolDirectoryPath
    {
        get
        {
            return poolConfigPathPrefix;
        }
    }
    private Dictionary<string, GameObjectPool> poolDict;
    private MyPoolManager()
    {
        GameObjectPoolList poolList = Resources.Load<GameObjectPoolList>(poolConfigPathMiddle);
        poolDict = new Dictionary<string, GameObjectPool>();
        if (poolList == null)
        {
            Debug.LogWarning("gameobjectpool is not exits!!!");
            return;
        }
        foreach (GameObjectPool pool in poolList.poolList)
        {
            poolDict.Add(pool.name, pool);
        }
    }
    public void Init()
    {
        //Do nothing
    }

    public GameObject GetInst(string poolName)
    {
        GameObjectPool pool;
        if (poolDict.TryGetValue(poolName, out pool))
        {
            return pool.GetInst();
        }
        Debug.LogWarning("Pool : " + poolName + " is not exits!!!"); 
        return null;
    }
}
