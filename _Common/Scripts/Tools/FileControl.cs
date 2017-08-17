using UnityEngine;
using System.IO;
using System;
using System.Linq;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
public class FileControl
{
    public static string GetStreamingAssetsPath(string fileName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return System.IO.Path.Combine(Application.streamingAssetsPath+ "/Android",fileName);
#elif UNITY_EDITOR
        return System.IO.Path.Combine("file://" + Application.streamingAssetsPath + "/Android", fileName);
#elif UNITY_IOS
        return  "file://"+ System.IO.Path.Combine(Application.streamingAssetsPath + "/IOS", fileName);
#else
        return "";    
#endif
    }
    public static void CreateDirectory(string path)
    {
        string directoryName = Path.GetDirectoryName(path);
        if (directoryName != "" && !Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);
    }
    public static void WriteAllText(string path, string content)
    {
        if (string.IsNullOrEmpty(path))
            return;
        CreateDirectory(path);
        File.WriteAllText(path, content);
        SetNoBackupFlag(path);
    }
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        if (string.IsNullOrEmpty(path))
            return;
        CreateDirectory(path);
        File.WriteAllBytes(path, bytes);
        SetNoBackupFlag(path);
    }
    public static void WriteAllBytesAsync(string path, byte[] bytes, Action action)
    {
        if (string.IsNullOrEmpty(path))
        {
            action();
            return;
        }
        ThreadHelper.Instance.QueueOnThreadPool((state) =>
        {
            WriteAllBytes(path, bytes);
            ThreadHelper.Instance.QueueOnMainThread(action);
        });
    }
    public static void SetNoBackupFlag(string path)
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            return;
#if UNITY_IOS
        //下载的AssetBundle需设置
        ThreadHelper.QueueOnMainThread(delegate ()
        {
            Device.SetNoBackupFlag(path);
        });
#endif
    }
    public static void Move(string sourceFileName, string destFileName)
    {
        if (sourceFileName == destFileName)
            return;
        if (!File.Exists(sourceFileName))
            return;
        CreateDirectory(destFileName);
        DeleteFile(destFileName);
        File.Move(sourceFileName, destFileName);
        SetNoBackupFlag(destFileName);
    }
    public static void Copy(string sourceFileName, string destFileName, bool overwrite)
    {
        if (sourceFileName == destFileName)
            return;
        CreateDirectory(destFileName);
        File.Copy(sourceFileName, destFileName, overwrite);
        SetNoBackupFlag(destFileName);
    }
    public static void CopyDirectory(string srcPath, string dstPath, bool overwrite = true, string excludeExtension = ".meta")
    {
        DeleteFile(dstPath);
        if (!Directory.Exists(dstPath))
            Directory.CreateDirectory(dstPath);
        foreach (var file in Directory.GetFiles(srcPath, "*.*").Where(path => Path.GetExtension(path) != excludeExtension))
        {
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), overwrite);
        }
        foreach (var dir in Directory.GetDirectories(srcPath).Where(path => Path.GetExtension(path) != excludeExtension))
            CopyDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)), overwrite);
    }
    public static void CopyDirectory(string srcPath, string dstPath, string[] excludeFileExtensions, string[] excludeDirectoryExtensions = null, bool overwrite = true)
    {
        if (!Directory.Exists(dstPath))
            Directory.CreateDirectory(dstPath);

        foreach (var file in Directory.GetFiles(srcPath, "*.*", SearchOption.TopDirectoryOnly).Where(path => excludeFileExtensions == null || !excludeFileExtensions.Contains(Path.GetExtension(path))))
        {
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), overwrite);
        }

        foreach (var dir in Directory.GetDirectories(srcPath).Where(path => excludeDirectoryExtensions == null || !excludeDirectoryExtensions.Contains(Path.GetExtension(path))))
            CopyDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)), excludeFileExtensions, excludeDirectoryExtensions, overwrite);
    }
    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }
    public static string GetFullPathWithoutExtension(string path)
    {
        return Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
    }
    public static int GetFileSize(string path)
    {
        if (!File.Exists(path))
            return 0;

        FileStream fs = new FileStream(path, FileMode.Open);
        long length = fs.Length;
        fs.Close();

        return (int)length;
    }
}
