#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif
namespace WZK
{
    /// <summary>
    /// 系统数据信息
    /// </summary>
    public class SystemData
    {
        /// <summary>
        /// 语言
        /// </summary>
        public static string LANGUAGE = "zh";
        /// <summary>
        /// 是否开发模式
        /// </summary>
#if UNITY_EDITOR
        public static bool IsDebugBuild = EditorUserBuildSettings.development;
#else
    public static bool IsDebugBuild = Debug.isDebugBuild;
#endif
    }
}
