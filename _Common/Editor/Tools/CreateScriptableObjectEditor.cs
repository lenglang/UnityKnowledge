using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class CreateScriptableObjectEditor
{
    static void 创建xxx配置()
    {
        
    }
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject);
        string assetPathName;
        if (string.IsNullOrEmpty(path))
        {
            path = Path.GetDirectoryName(SceneManager.GetActiveScene().path);
            if (string.IsNullOrEmpty(path))
            {
                assetPathName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + typeof(T).ToString() + ".asset");
            }
            else
            {
                assetPathName = AssetDatabase.GenerateUniqueAssetPath(path+"/" + typeof(T).ToString() + ".asset");
            }
            
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
