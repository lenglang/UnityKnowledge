using UnityEngine;
using System.Collections;

public static class GameObjectUtility  
{
    /// <summary>
    /// 设定层
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layer"></param>
    public static void SetLayer(this GameObject obj, int layer)
    {
        obj.layer = layer;
        if (obj.transform.childCount == 0) return;
        foreach (Transform child in obj.transform)
        {
            child.gameObject.layer = layer;
        }
    }
}
