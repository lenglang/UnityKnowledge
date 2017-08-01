using Common.LoadScene;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneControl
{
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    public static void LoadScene(string sceneName,string LoadingSceneName="Loading")
    {
        Resources.UnloadUnusedAssets();
        SceneDataModel.NextScene = sceneName;
        SceneManager.LoadScene(LoadingSceneName);
    }
    /// <summary>
    /// 返回主场景
    /// </summary>
    public static void BackMain()
    {
        SoundControl.Instance.DestroyAllSound();
        //LoadScene("Main");
        //SoundControl.Instance.PlayFromResource("音效/", SoundType.按钮音效);
    }
}
