using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static bool activeSelf
    {
        get
        {
            return Instance.gameObject.activeSelf;
        }
        set
        {
            Instance.gameObject.SetActive(value);
        }
    }

    public static bool Enabled
    {
        get
        {
            return Instance.enabled;
        }
        set
        {
            Instance.enabled = value;
        }
    }

    public static bool isNullOfInstance
    {
        get
        {
            if (_instance == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /**
       Returns the instance of this singleton.
    */
    private static T _instance;
    public static T Instance
    {
        private set
        {
            if(_instance != null && value != null)
            {
                DebugBuild.Log(typeof(T) + " Instance is not null");
                return;
            }
            _instance = value;
        }
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<T>();
            if (_instance == null)
                Debug.LogError(typeof(T) + " instance is null");
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        Instance = this as T;
    }
    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}