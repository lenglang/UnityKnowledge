using UnityEngine;
using System.Collections;

public class GlobalManager : MonoBehaviour {

    private static GlobalManager instance;
    public static GlobalManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("GlobalManager");
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<GlobalManager>();
            }
            return instance;
        }
    }
}
