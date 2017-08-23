using Common.LoadScene;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace WZK
{
    public class LoadSceneManager
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="nextScene">加载场景名</param>
        /// <param name="isAssetBundleScene">是否AssetBundle场景</param>
        /// <param name="isStreamingAssets">是否在StreamingAssets文件夹下</param>
        public static void LoadScene(SceneType nextScene, bool isAssetBundleScene = false, bool isStreamingAssets = false)
        {
            Resources.UnloadUnusedAssets();
            SceneDataModel.NextScene = nextScene;
            SceneDataModel.IsAssetBundleScene = isAssetBundleScene;
            SceneDataModel.IsStreamingAssets = isStreamingAssets;
            SceneManager.LoadScene(SceneType.加载页面.GetEnumDescription());

        }
    }
}
