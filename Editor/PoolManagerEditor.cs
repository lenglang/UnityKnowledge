using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class PoolManagerEditor  {
    [MenuItem("对象池/创建对象池")]
    static void CreateGameObjectPoolList()
    {
        //GameObjectPoolList poolList = ScriptableObject.CreateInstance<GameObjectPoolList>();
        //string filePath = PoolManager.PoolConfigPath;
        //string directoryPath=PoolManager.PoolDirectoryPath;
        //if (Directory.Exists(directoryPath) == false) Directory.CreateDirectory(directoryPath);
        //if (File.Exists(filePath))
        //{
        //    Debug.LogError("已存在对象池");
        //    return;
        //}
        //AssetDatabase.CreateAsset(poolList,filePath);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        //EditorUtility.FocusProjectWindow();
        //Selection.activeObject = poolList;
        //Debug.Log("对象池文件已创建：" + filePath);
        CreateAsset<GameObjectPoolList>();
    }
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        string assetPathName;
        if (string.IsNullOrEmpty(path))
        {
            assetPathName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + typeof(T).ToString() + ".asset");
        }
        else
        {
            path = path.Substring(0, path.LastIndexOf("/") + 1);
            assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + typeof(T).ToString() + ".asset");
        }
        AssetDatabase.CreateAsset(asset, assetPathName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
