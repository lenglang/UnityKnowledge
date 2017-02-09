using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class PoolManagerEditor  {
    [MenuItem("对象池/创建对象池")]
    static void CreateGameObjectPoolList()
    {
        GameObjectPoolList poolList = ScriptableObject.CreateInstance<GameObjectPoolList>();
        string filePath = PoolManager.PoolConfigPath;
        string directoryPath=PoolManager.PoolDirectoryPath;
        if (Directory.Exists(directoryPath) == false) Directory.CreateDirectory(directoryPath);
        if (File.Exists(filePath))
        {
            Debug.LogError("已存在对象池");
            return;
        }
        AssetDatabase.CreateAsset(poolList,filePath);
        AssetDatabase.SaveAssets();
        Debug.Log("对象池文件已创建：" + filePath);
    }
}
