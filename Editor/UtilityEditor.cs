using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.IO.Compression;
using System.Reflection;
using LitJson;
using UnityEngine.UI;
public class UtilityEditor : Editor
{
    [MenuItem("小工具/修复Shader")]
    private static void 修复Shader()
    {
        var renderers = Resources.FindObjectsOfTypeAll<Renderer>();
        if (renderers == null || renderers.Length == 0)
            return;

        foreach (var renderer in renderers)
        {
            if (AssetDatabase.GetAssetPath(renderer) != "")
                continue;

            foreach (var sharedMaterial in renderer.sharedMaterials)
            {
                if (sharedMaterial != null)
                    sharedMaterial.shader = Shader.Find(sharedMaterial.shader.name);
            }
        }

        var graphics = Resources.FindObjectsOfTypeAll<Graphic>();
        if (graphics == null || graphics.Length == 0)
            return;

        foreach (var graphic in graphics)
        {
            if (AssetDatabase.GetAssetPath(graphic) != "")
                continue;

            graphic.material.shader = Shader.Find(graphic.material.shader.name);
        }
    }
    [MenuItem("小工具/清空持久化数据")]
    static void 清空持久化数据()
    {
        Directory.Delete(Application.persistentDataPath, true);
        PlayerPrefs.DeleteAll();
    }
    [MenuItem("小工具/一键查找贴图")]
    static void 一键查找贴图()
    {
        var assets = AssetDatabase.FindAssets("t:Texture2D");
        Debug.Log(assets.Length);

        foreach (var guid in assets)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (textureImporter == null)
                continue;

            if (textureImporter.textureType == TextureImporterType.Advanced)
            {
                Debug.Log(assetPath);
            }
        }
    }
    [MenuItem("小工具/从图集中导出精灵")]
    static void 从图集中导出精灵()
    {
        foreach (var activeObject in Selection.objects)
        {
            var texture = activeObject as Texture2D;

            if (texture == null)
                continue;

            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            var directoryName = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath);

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            foreach (var spriteMetaData in textureImporter.spritesheet)
            {
                var rect = spriteMetaData.rect;
                var x = (int)rect.x;
                var y = (int)rect.y;
                var width = (int)rect.width;
                var height = (int)rect.height;

                var colors = texture.GetPixels(x, y, width, height);

                var tex = new Texture2D(width, height);
                tex.SetPixels(colors);
                var bytes = tex.EncodeToPNG();

                var path = directoryName + "/" + spriteMetaData.name + ".png";

                File.WriteAllBytes(path, bytes);
            }

            AssetDatabase.Refresh();

            AssetDatabase.StartAssetEditing();

            foreach (var spriteMetaData in textureImporter.spritesheet)
            {
                var path = directoryName + "/" + spriteMetaData.name + ".png";

                var texImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                texImporter.textureType = TextureImporterType.Sprite;
                texImporter.SaveAndReimport();
            }

            AssetDatabase.StopAssetEditing();
        }
    }

    [MenuItem("小工具/重新合成图集")]
    static void 重新合成图集()
    {
        foreach (var activeObject in Selection.objects)
        {
            var texture = activeObject as Texture2D;

            if (texture == null)
                continue;

            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            var directoryName = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath);

            if (!Directory.Exists(directoryName))
            {
                Debug.LogError("!Directory.Exists(" + directoryName + ")");
                continue;
            }

            var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            foreach (var spriteMetaData in textureImporter.spritesheet)
            {
                var rect = spriteMetaData.rect;
                var x = (int)rect.x;
                var y = (int)rect.y;
                var width = (int)rect.width;
                var height = (int)rect.height;

                var path = directoryName + "/" + spriteMetaData.name + ".png";

                if (!File.Exists(path))
                    continue;

                var bytes = File.ReadAllBytes(path);

                var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
                if (!tex.LoadImage(bytes))
                {
                    Debug.LogError("!tex.LoadImage");
                    continue;
                }

                var colors = tex.GetPixels();

                texture.SetPixels(x, y, width, height, colors);
            }

            File.WriteAllBytes(assetPath, texture.EncodeToPNG());

            AssetDatabase.Refresh();
        }
    }
    //[MenuItem("小工具/一键修改模型动画压缩模式")]
    //static void 一键修改模型动画压缩模式()
    //{
    //    var assets = AssetDatabase.FindAssets("t:GameObject");
    //    Debug.Log(assets.Length);

    //    AssetDatabase.StartAssetEditing();

    //    foreach (var guid in assets)
    //    {
    //        var assetPath = AssetDatabase.GUIDToAssetPath(guid);
    //        var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
    //        if (modelImporter == null)
    //            continue;
    //        if (modelImporter.meshCompression != ModelImporterMeshCompression.High)
    //        {
    //            modelImporter.meshCompression = ModelImporterMeshCompression.High;

    //            modelImporter.SaveAndReimport();
    //        }
    //    }

    //    AssetDatabase.StopAssetEditing();

    //    AssetDatabase.Refresh();
    //}
    //[MenuItem("小工具/一键修改贴图压缩模式")]
    //static void 一键修改贴图压缩模式()
    //{
    //    var assets = AssetDatabase.FindAssets("t:Texture2D");
    //    Debug.Log(assets.Length);

    //    foreach (var guid in assets)
    //    {
    //        var assetPath = AssetDatabase.GUIDToAssetPath(guid);
    //        var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
    //        if (textureImporter == null)
    //            continue;


    //    }
    //}
}


