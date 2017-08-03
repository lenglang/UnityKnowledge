using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Common.LoadScene
{
    public class LoadingControl : MonoBehaviour
    {
        /// <summary>
        /// 进度增加平滑值
        /// </summary>
        public int _smooth = 100;
        void Start()
        {
            StartCoroutine("StartLoading",SceneDataModel.NextScene);
        }
        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        private IEnumerator StartLoading(SceneType sceneName)
        {
            int displayProgress = 0;
            int progress = 0;
            UpdateProgress(progress);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.GetEnumDescription());
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                progress = (int)asyncOperation.progress * 100;
                while (displayProgress < progress)
                {
                    displayProgress+=_smooth;
                    UpdateProgress(displayProgress);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            progress = 100;
            while (displayProgress < progress)
            {
                displayProgress +=_smooth;
                UpdateProgress(displayProgress);
                yield return new WaitForEndOfFrame();
            }
            SceneDataModel.CurrentScene = sceneName;
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
