using Common.LoadScene;
using UnityEngine;
using UnityEngine.UI;
namespace WZK
{
    public class LoadingViewExample : LoadingManager
    {
        [Header("文本进度")]
        public Text _textProgress;
        [Header("图片进度")]
        public Image _imageProgress;
        private void Awake()
        {
            _smooth = 100;//进度增量平滑值，越大越不平滑，默认100即不平滑
        }
        public override void UpdateProgress(int value)
        {
            base.UpdateProgress(value);
            _textProgress.text = value + "%";
            _imageProgress.fillAmount = (float)value / 100f;
        }
    }
}
