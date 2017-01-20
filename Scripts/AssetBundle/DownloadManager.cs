using UnityEngine;
using System.Collections;

public class DownloadManager
{
    private static DownloadManager _instance;//单例
    private string platform = "";//当前平台
    /// <summary>
    /// 获取加载管理单例
    /// </summary>
    /// <returns></returns>
    public static DownloadManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DownloadManager();
            _instance.Init();
        }
        return _instance;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
#if UNITY_WEBGL
            platform = "WebGL";
#elif UNITY_ANDROID
        platform = "Android";
#elif UNITY_IOS
            platform = "iOS";
#endif
    }
    public void StartDownload()
    {
        Debug.Log(platform);
    }
}
