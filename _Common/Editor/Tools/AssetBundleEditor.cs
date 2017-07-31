using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class AssetBundleEditor : Editor
{

    [MenuItem("AssetBundle/打包选中文件为一个AssetBundle(可多选)")]
    static void BuildAssetBundle()
    {
        if (Selection.objects.Length == 0)
            return;
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            Build(Selection.objects[i]);
        }
        Debug.LogError("共打包" + Selection.objects.Length + "个AssetBundle");
    }
    static void Build(Object obj)
    {
        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetNames = new string[Selection.objects.Length];
        assetBundleBuild.assetNames[0] = AssetDatabase.GetAssetPath(obj);
        string directoryName = Path.GetDirectoryName(assetBundleBuild.assetNames[0]);
        assetBundleBuild.assetBundleName = Path.GetFileName(assetBundleBuild.assetNames[0]) + ".unity3d";

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
}
