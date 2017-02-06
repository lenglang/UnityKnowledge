using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
public class AssetBundleCreate : Editor {

    [MenuItem("打包AssetBundle/本地测试资源")]
    public static void BuildResources()
    {
        if (Selection.objects.Length == 0)
            return;
        //Debug.LogError(Application.persistentDataPath);
        Object[] seleObjects = Selection.objects;
        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetNames = new string[seleObjects.Length];
        for (int i = 0; i < seleObjects.Length; i++)
        {
            assetBundleBuild.assetNames[i] = AssetDatabase.GetAssetPath(seleObjects[i]);
        }
        //目录名字
        //string directoryName = Path.GetDirectoryName(assetBundleBuild.assetNames[0]);
        //Debug.LogError(directoryName);
        if (seleObjects.Length == 1)
            assetBundleBuild.assetBundleName = Path.GetFileName(assetBundleBuild.assetNames[0])+".txt";
        else
            assetBundleBuild.assetBundleName = Path.GetFileName(assetBundleBuild.assetNames[0])+".txt";
#if UNITY_ANDROID
        string buildPath = Application.dataPath+ "/Resources" + "/Android";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        BuildPipeline.BuildAssetBundles(buildPath,new AssetBundleBuild[] { assetBundleBuild },BuildAssetBundleOptions.ChunkBasedCompression,BuildTarget.Android);
#endif

#if UNITY_IOS
		string buildPath = Application.dataPath+ "/Resources"+"/IOS";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        BuildPipeline.BuildAssetBundles(buildPath,new AssetBundleBuild[] { assetBundleBuild },BuildAssetBundleOptions.ChunkBasedCompression,BuildTarget.iOS);
#endif

    }
    [MenuItem("打包AssetBundle/网络加载资源")]
    static void BuildAssetBundle()
    {
        Debug.LogError("打包成package需将后缀unity删除");
        if (Selection.objects.Length == 0)
            return;
        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetNames = new string[Selection.objects.Length];
        for (int i = 0; i < Selection.objects.Length; i++)
            assetBundleBuild.assetNames[i] = AssetDatabase.GetAssetPath(Selection.objects[i]);

        string directoryName = Path.GetDirectoryName(assetBundleBuild.assetNames[0]);
        //Debug.Log(directoryName);
        directoryName = "Assets/AssetBundle";
        if (Selection.objects.Length == 1)
            assetBundleBuild.assetBundleName = Path.GetFileName(assetBundleBuild.assetNames[0]) + ".unity3d";
        else
            assetBundleBuild.assetBundleName = Path.GetFileName(directoryName) + ".unity3d";

#if UNITY_ANDROID
        directoryName += "/Android";
#endif
#if UNITY_IOS
        directoryName += "/iOS";
#endif
        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

#if UNITY_ANDROID
        BuildPipelineHelper.BuildAssetBundles(directoryName, new AssetBundleBuild[] { assetBundleBuild }, BuildAssetBundleOptions.None, BuildTarget.Android);
#endif

#if UNITY_IOS
        BuildPipelineHelper.BuildAssetBundles(directoryName, new AssetBundleBuild[] {assetBundleBuild}, BuildAssetBundleOptions.None, BuildTarget.iOS);
#endif
    }

    [MenuItem("测试/AssetBundle资源包含的资源名输出")]
    static void LoadAllAssets()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!File.Exists(path))
            return;
        Debug.Log(path);
       AssetBundle assetBundle=AssetBundle.LoadFromMemory(File.ReadAllBytes(path));
        if (assetBundle != null)
        {
            foreach (var name in assetBundle.GetAllAssetNames())
            {
                Debug.Log(name);
            }
            assetBundle.Unload(false);
        }
    }
}
