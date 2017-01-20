using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ResourceCompress :Editor {
    [MenuItem("资源压缩|解压/资源压缩")]
    static void 资源压缩()
    {
        Debug.LogError("需导入宝宝巴士框架");
        MultiFilePacking("压缩后的包名");
    }
    static void MultiFilePacking(string name)
    {

        //if (Selection.objects.Length == 0)
        //    return;

        //Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
        //foreach (var asset in Selection.objects)
        //{
        //    Debug.Log(asset);
        //    string assetPath = AssetDatabase.GetAssetPath(asset);

        //    string path = Path.GetFullPath(assetPath);
        //    string filename = Path.GetFileName(path);

        //    files[filename] = File.ReadAllBytes(path);
        //}

        //if (Selection.objects.Length > 1)
        //{
        //    string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //    string path = Path.GetDirectoryName(assetPath) + "/" + name;
        //    BinaryUtility.SerializeBinary(path, files);
        //}
    }
    [MenuItem("资源压缩|解压/包含资源名输出")]
    static void MultiFileDecoderName()
    {
        Debug.LogError("需导入宝宝巴士框架");
        //if (Selection.objects.Length == 0)
        //    return;
        //string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //string extension = Path.GetExtension(assetPath);
        //if (extension != ".package")
        //    return;
        //var files = Utility.DeserializeBinary<Dictionary<string, byte[]>>(assetPath);
        //foreach (var file in files)
        //    Debug.Log(file.Key);
    }

    [MenuItem("资源压缩|解压/资源解压")]
    static void MultiFileDecoder()
    {
        Debug.LogError("需导入宝宝巴士框架");
        if (Selection.objects.Length == 0)
            return;
        //string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //string extension = Path.GetExtension(assetPath);
        //if (extension != ".package")
        //    return;
        //var files = Utility.DeserializeBinary<Dictionary<string, byte[]>>(assetPath);
        //foreach (KeyValuePair<string, byte[]> pair in files)
        //Utility.WriteAllBytes(GetGameFullPath(pair.Key), pair.Value);
        return;
        //Debug.Log(AssetDatabase.GetAssetPath(Selection.activeObject));
        //string buildPath = Application.dataPath + "/Resources" + "/Android";
        //if (!Directory.Exists(buildPath))
        //{
        //    Directory.CreateDirectory(buildPath);
        //}
    }
}
