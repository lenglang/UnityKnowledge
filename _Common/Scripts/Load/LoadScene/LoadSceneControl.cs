using Common.LoadScene;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneControl
{
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneType"></param>
    /// <param name="loadingSceneName"></param>
    public static void LoadScene(SceneType nextScene)
    {
        Resources.UnloadUnusedAssets();
        SceneDataModel.NextScene = nextScene;
        SceneManager.LoadScene(SceneType.加载页面.GetEnumDescription());
        
    }
}
