using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WZK;
public class ResourcesEditor:Editor {
    [MenuItem("Tools/分别打包选中文件为一个AssetBundle")]
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
    [MenuItem("Tools/.unity3d资源大小配置表生成")]
    static void CreateUnity3DConfig()
    {
        CreateConfig("*.unity3d");
    }
    [MenuItem("Tools/.png资源大小配置表生成")]
    static void CreatePNGConfig()
    {
        CreateConfig("*.png");
    }
    [MenuItem("Tools/.mp3资源大小配置表生成")]
    static void CreateMP3Config()
    {
        CreateConfig("*.mp3");
    }
    [MenuItem("Tools/所有资源大小配置表生成")]
    static void CreateAllConfig()
    {
        CreateConfig("*");
    }
    static void CreateConfig(string extension)
    {
        if (Selection.objects.Length == 0)
            return;
        string path = AssetDatabase.GetAssetPath(Selection.objects[0]);
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        string[] files = Directory.GetFiles(path, extension);
        foreach (string file in files)
        {
            //string fileName = Path.GetFileName(file);
            string fileName = Path.GetFileNameWithoutExtension(file);
            dictionary.Add(fileName, File.ReadAllBytes(file).Length);
        }
        string[] strs = path.Split('/');
        path = "";
        for (int i = 1; i < strs.Length; i++)
        {
            path += strs[i];
            if (i != strs.Length - 1)
            {
                path += "/";
            }
        }
        path = Application.dataPath + "/" + path + "/资源大小配置.xml";
        XmlUtility.SerializeXmlToFile(path, dictionary.ToList());
        Debug.Log(path);
        AssetDatabase.Refresh();
    }
}
