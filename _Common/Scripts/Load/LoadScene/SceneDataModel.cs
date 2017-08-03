namespace Common.LoadScene
{
    public enum SceneType
    {
        加载页面,
        主场景,
        擦除效果
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
    }
}
