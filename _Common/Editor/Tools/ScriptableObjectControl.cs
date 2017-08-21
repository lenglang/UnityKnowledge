using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
namespace WZK
{
    /// <summary>
    /// 序列化脚本对象控制
    /// </summary>
    public class ScriptableObjectControl
    {
        //[MenuItem("Assets/Create/自定义/创建序列化脚本")]
        public static void CreateScriptableObject()
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
                    assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).ToString() + ".asset");
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
}
