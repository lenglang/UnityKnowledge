#define LocalTest

using UnityEngine;
using System.Collections;
using System.IO;


public class AssetBundleLoader : MonoBehaviour
{
    [HideInInspector]
    public static AssetBundle assetBundle;

    private static AssetBundleLoader main;

    public static string loadPath;
    public static AssetBundleLoader Main
    {
        get
        {
            if (main == null)
            {
                GameObject a = new GameObject("AssetBundleLoader");
                main = a.AddComponent<AssetBundleLoader>();
            }
            return main;
        }
    }

    public void LoadAssetBundle(string name, System.Action completeAction)
    {
        assetBundle = null;
        string path = "";
#if UNITY_ANDROID
        path = "Android/";
#elif UNITY_IOS
        path = "IOS/";
#endif
#if UNITY_EDITOR &&  LocalTest
        Debug.Log(path + name);
        assetBundle = AssetBundle.LoadFromMemory(Resources.Load<TextAsset>(path + name).bytes);
        //assetBundle = GetAssetBundle();
        if (completeAction != null)
            completeAction();
#endif

#if UNITY_EDITOR && !LocalTest
        assetBundle = GetAssetBundle();
        if (completeAction != null)
            completeAction();
#endif

#if !UNITY_EDITOR && LocalTest
        StartCoroutine(LoadAsset(name, completeAction));
#endif


#if UNITY_ANDROID && !UNITY_EDITOR && !LocalTest
        //assetBundle = MemoryPrefs.GetObject<AssetBundle>("SceneAssetBundle");
         assetBundle = GetAssetBundle();
                if (completeAction != null)
                completeAction();
#endif
#if UNITY_IOS && !UNITY_EDITOR && !LocalTest
        //assetBundle = MemoryPrefs.GetObject<AssetBundle>("SceneAssetBundle");
        assetBundle = GetAssetBundle();
                if (completeAction != null)
                completeAction();
#endif

    }
    IEnumerator LoadAsset(string name, System.Action completeAction)
    {
        string path = null;
        if (Application.platform == RuntimePlatform.Android)
        {
            path = "Android/";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = "IOS/";
        }

        assetBundle = AssetBundle.LoadFromMemory(Resources.Load<TextAsset>(path + name).bytes);

        if (completeAction != null)
            completeAction();

        yield return null;
    }
    public static T Load<T>(string name) where T : UnityEngine.Object
    {
        T asset = default(T);

        if (assetBundle == null)
        {
            Debug.LogError("找不到Assetbundle");
            return asset;
        }

        asset = assetBundle.LoadAsset<T>(name);
        return asset;
    }
    public static void Unload()
    {
        if (assetBundle != null)
        {
            assetBundle.Unload(true);
            assetBundle = null;
        }
    }

    AssetBundle GetAssetBundle()
    {
        if (!File.Exists(loadPath))
            return null;
        return AssetBundle.LoadFromMemory(File.ReadAllBytes(loadPath));
    }
}
