using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace WZK
{
    public class LoadingManager : MonoBehaviour
    {
        /// <summary>
        /// 进度增加平滑值
        /// </summary>
        public int _smooth = 100;
        void Start()
        {
            UpdateProgress(0);
            if (SceneDataModel.IsAssetBundleScene)
            {
                string persistentDataPath = Application.persistentDataPath + "/" + SceneDataModel.ScenePath + SceneDataModel.NextScene.GetEnumDescription() + ".unity3d";
                if (SceneDataModel.IsStreamingAssets && !File.Exists(persistentDataPath))
                {
                    StartCoroutine("StartLoadingStreamingAssets", persistentDataPath);
                }
                else
                {
                    this.CreateAssetBundleFromFile(persistentDataPath, delegate { StartCoroutine("StartLoadingScene"); });
                }
            }
            else
            {
                StartCoroutine("StartLoadingScene");
            }
        }
        /// <summary>
        /// 下载StreamingAssets下场景到持久化路径下
        /// </summary>
        /// <param name="persistentDataPath"></param>
        /// <returns></returns>
        private IEnumerator StartLoadingStreamingAssets(string persistentDataPath)
        {
            string streamingAssetsPath = FileManager.GetStreamingAssetsPath(SceneDataModel.ScenePath + SceneDataModel.NextScene.GetEnumDescription()+".unity3d");
            Debug.Log(streamingAssetsPath);
            WWW www = new WWW(streamingAssetsPath);
            yield return www;
            FileManager.CreateDirectory(Application.persistentDataPath + "/" + SceneDataModel.ScenePath);
            File.WriteAllBytes(persistentDataPath, www.bytes);
            this.CreateAssetBundleFromFile(persistentDataPath, delegate { StartCoroutine("StartLoadingScene"); });
        }
        /// <summary>
        /// 开始加载场景
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartLoadingScene()
        {
            int displayProgress = 0;
            int progress = 0;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneDataModel.NextScene.GetEnumDescription());
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                progress = (int)asyncOperation.progress * 100;
                while (displayProgress < progress)
                {
                    displayProgress += _smooth;
                    UpdateProgress(displayProgress);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            progress = 100;
            while (displayProgress < progress)
            {
                displayProgress += _smooth;
                UpdateProgress(displayProgress);
                yield return new WaitForEndOfFrame();
            }
            SceneDataModel.CurrentScene = SceneDataModel.NextScene;
            asyncOperation.allowSceneActivation = true;
            yield return asyncOperation;
        }
        /// <summary>
        /// 进度更新
        /// </summary>
        /// <param name="value"></param>
        public virtual void UpdateProgress(int value)
        {
            if (value > 100) value = 100;
        }
    }
}
