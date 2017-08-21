using System.ComponentModel;

namespace WZK
{
    public enum SceneType
    {
        加载页面,
        主场景,
        擦除效果,
        [Description("wanjucheng")]
        玩具城
    }
    public class SceneDataModel
    {
        /// <summary>
        /// 下一个场景
        /// </summary>
        public static SceneType NextScene { get; set; }
        /// <summary>
        /// 当前场景
        /// </summary>
        public static SceneType CurrentScene { get; set; }
        /// <summary>
        /// 是否是AssetBundle场景资源
        /// </summary>
        public static bool IsAssetBundleScene = false;
        /// <summary>
        /// 是否在StreamingAssets文件夹下
        /// </summary>
        public static bool IsStreamingAssets = false;
        /// <summary>
        /// 场景路径
        /// </summary>
        public static string ScenePath = "Scenes/";
    }
}
